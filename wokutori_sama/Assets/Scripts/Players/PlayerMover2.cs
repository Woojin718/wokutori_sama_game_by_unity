using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(PlayerCore))]
[RequireComponent(typeof(PlayerInputProvider))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover2 : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float carrySpeed = 3f / 4f;
    [SerializeField] private float jumpImpulse;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private string carryTag = "Carriable";
    [SerializeField] private Collider2D playerCollider;
    [SerializeField] private Collider2D forGroundTrigger;
    [SerializeField] private Collider2D forCarryTrigger;
    [SerializeField] private Collider2D forHoldTrigger;
    [SerializeField] private float boxFallAngle = 3f;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isFacingLeft = new BoolReactiveProperty();
    public IReadOnlyReactiveProperty<bool> IsFacingLeft => isFacingLeft;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isWalking = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsWalking => isWalking;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isJumpingUp = new BoolReactiveProperty(false);
    [ReadOnly] [SerializeField] private BoolReactiveProperty isJumpingDown = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsJumpingUp => isJumpingUp;
    public IReadOnlyReactiveProperty<bool> IsJumpingDown => isJumpingDown;

    [ReadOnly] [SerializeField] private BoolReactiveProperty onGround = new BoolReactiveProperty(true);
    public IReadOnlyReactiveProperty<bool> OnGround => onGround;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isPushing = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsPushing => isPushing;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isPulling = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsPulling => isPulling;

    public bool IsCarrying => IsPushing.Value || IsPulling.Value;
    public bool IsJumping => IsJumpingUp.Value || IsJumpingDown.Value;
    public bool IsMoving => IsWalking.Value || IsJumping;

    private PlayerCore core;
    private Rigidbody2D rb;
    [ReadOnly] [SerializeField] private Rigidbody2D carryRigidbody;
    [ReadOnly] [SerializeField] private Collider2D carryCollider;
    private bool targetExistsLeft => carryRigidbody.transform.position.x - transform.position.x < 0f;

    private void Start()
    {
        core = GetComponent<PlayerCore>();
        rb = GetComponent<Rigidbody2D>();

        var inputProvider = GetComponent<PlayerInputProvider>();

        // 左右の移動処理
        // IsWalkingの更新
        inputProvider
            .MoveInput
            // 地面にいるかまたはジャンプアップ中の時
            .Where(_ => OnGround.Value || IsJumpingUp.Value)
            .Subscribe(inputValue =>
            {
                var velocity = rb.velocity;
                var moveSpeed = IsCarrying ? carrySpeed : walkSpeed;
                rb.velocity = new Vector2(moveSpeed * inputValue, velocity.y);

                isWalking.Value = !Mathf.Approximately(inputValue, 0f);
            })
            .AddTo(this);

        // 運搬処理
        inputProvider
            .MoveInput
            .Where(_ => IsCarrying)
            .Subscribe(inputValue =>
            {
                if (!forHoldTrigger.IsTouching(carryCollider))
                {
                    FinishCarry();
                    carryRigidbody = null;
                    carryCollider = null;
                    return;
                }

                if (carryRigidbody.bodyType != RigidbodyType2D.Dynamic)
                {
                    carryRigidbody.bodyType = RigidbodyType2D.Dynamic;
                }

                var carryVelocity = carryRigidbody.velocity;
                carryRigidbody.velocity = new Vector2(carrySpeed * inputValue, carryVelocity.y);

                if (targetExistsLeft == inputValue < 0f)
                {
                    isPushing.Value = true;
                    isPulling.Value = false;
                }
                else
                {
                    isPushing.Value = false;
                    isPulling.Value = true;
                }
            })
            .AddTo(this);

        // IsFacingLeftの更新
        inputProvider
            .MoveInput
            // 歩いているかつ物を運んでいない時
            .Where(_ => IsWalking.Value && !IsCarrying)
            .Subscribe(inputValue => isFacingLeft.Value = inputValue < 0f)
            .AddTo(this);

        var token = this.GetCancellationTokenOnDestroy();

        // ジャンプ開始判定
        inputProvider
            .OnJumpInput
            .Where(_ => OnGround.Value && !IsCarrying && !IsJumping)
            .Subscribe(_ => JumpFlow(token).Forget())
            .AddTo(this);

        // 接地判定
        Observable.EveryFixedUpdate()
            .Select(_ => forGroundTrigger.IsTouchingLayers(groundLayer))
            .Subscribe(onGround => this.onGround.Value = onGround)
            .AddTo(this);

        // 運搬開始判定
        inputProvider
            .CarryInput
            .Where(carryStart => carryStart)
            .Where(_ => !IsCarrying)
            .Where(_ => OnGround.Value)
            .Where(_ => carryRigidbody != null)
            .Subscribe(_ =>
            {
                // IsFacingLeftの更新
                isFacingLeft.Value = targetExistsLeft;
                isPushing.Value = true;
                isPulling.Value = false;
            })
            .AddTo(this);

        // 運搬終了判定
        inputProvider
            .CarryInput
            .Where(carryStart => !carryStart)
            .Where(_ => IsCarrying)
            .Subscribe(_ => FinishCarry())
            .AddTo(this);

        // 運搬物取得
        forCarryTrigger.gameObject
            .OnTriggerEnter2DAsObservable()
            .Where(_ => carryRigidbody == null)
            .Where(col => col.CompareTag(carryTag))
            .Subscribe(col =>
            {
                var cullingObject = col.GetComponent<LampCullingObject>();
                if (cullingObject != null && cullingObject.IsCulling)
                {
                    return;
                }

                carryRigidbody = col.GetComponent<Rigidbody2D>();
                carryCollider = col.GetComponent<Collider2D>();
                carryRigidbody.bodyType = RigidbodyType2D.Static;
            })
            .AddTo(this);

        // 運搬物廃棄
        forCarryTrigger.gameObject
            .OnTriggerExit2DAsObservable()
            .Where(_ => !IsCarrying)
            .Where(col => carryRigidbody != null && col.transform == carryRigidbody.transform)
            .Subscribe(col =>
            {
                FinishCarry();
                carryRigidbody = null;
                carryCollider = null;
            })
            .AddTo(this);

        core.OnPlayerDied
            .Subscribe(_ =>
            {
                var colliders = new Collider2D[rb.attachedColliderCount];
                rb.GetAttachedColliders(colliders);
                foreach (var col in colliders)
                {
                    col.isTrigger = true;
                }

                rb.bodyType = RigidbodyType2D.Static;
            })
            .AddTo(this);

        core.CanMove
            .SkipLatestValueOnSubscribe()
            .Subscribe(canMove => rb.bodyType = canMove ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static)
            .AddTo(this);
    }

    // 運搬廃棄処理
    private void FinishCarry()
    {
        isPushing.Value = false;
        isPulling.Value = false;

        // 地上にある状態で離した時
        if (Mathf.Abs(carryRigidbody.transform.eulerAngles.z) < boxFallAngle)
        {
            carryRigidbody.bodyType = RigidbodyType2D.Static;
        }
        // 既に傾き始めている時
        else
        {
            var carriableBox = carryRigidbody.GetComponent<CarriableBox>();
            carriableBox.IsFalling = true;
        }
    }

    private async UniTask JumpFlow(CancellationToken token)
    {
        isJumpingUp.Value = true;
        rb.AddForce(new Vector2(0f, jumpImpulse), ForceMode2D.Impulse);

        await UniTask.WaitWhile(() => rb.velocity.y > 0f, cancellationToken: token);

        isJumpingUp.Value = false;
        isJumpingDown.Value = true;

        // Stageレイヤーで接地判定
        await UniTask.WhenAny(
            OnGround
                .Where(onGround => onGround)
                .Take(1)
                .ToUniTask(cancellationToken: token),
            UniTask.WaitWhile(() => Mathf.Approximately(rb.velocity.y, 0f), cancellationToken: token));

        isJumpingDown.Value = false;
    }
}