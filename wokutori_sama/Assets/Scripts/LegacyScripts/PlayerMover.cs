using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(PlayerInputProvider))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float carrySpeed = 0.25f;
    [SerializeField] private float jumpImpulse;
    [SerializeField] private Transform leftPushPoint;
    [SerializeField] private Transform rightPushPoint;
    [SerializeField] private Transform bottomPoint;

    [SerializeField] private Transform rightGroundPoint;
    [SerializeField] private Transform leftGroundPoint;

    [SerializeField] private LayerMask groundLayer;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isFacingLeft = new BoolReactiveProperty(true);
    public IReadOnlyReactiveProperty<bool> IsFacingLeft => isFacingLeft;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isCarryingLeft = new BoolReactiveProperty(true);
    public IReadOnlyReactiveProperty<bool> IsCarryingLeft => isCarryingLeft;

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

    public bool IsCarrying => isPushing.Value || isPulling.Value;

    private Rigidbody2D rb;
    private Transform carryTarget;
    private Rigidbody2D carryRigidbody;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var inputProvider = GetComponent<PlayerInputProvider>();

        inputProvider
            .MoveInput
            // 地面についているかまたはジャンプ中の時
            .Where(_ => onGround.Value || isJumpingUp.Value)
            .SubscribeWithState((transform, walkSpeed, isFacingLeft, isWalking, rb),
                (inputValue, tuple) =>
                {
                    var velocity = tuple.rb.velocity;
                    var moveSpeed = IsCarrying ? carrySpeed : walkSpeed;
                    tuple.rb.velocity = new Vector2(moveSpeed * inputValue, velocity.y);

                    tuple.isWalking.Value = !Mathf.Approximately(inputValue, 0f);

                    if (IsCarrying)
                    {
                        var carryVelocity = carryRigidbody.velocity;
                        carryRigidbody.velocity = new Vector2(carrySpeed * inputValue, carryVelocity.y);
                    }
                })
            .AddTo(this);

        inputProvider
            .MoveInput
            // 地面についているかまたはジャンプ中の時
            .Where(_ => onGround.Value || isJumpingUp.Value)
            .Where(input => IsWalking.Value)
            .Where(_ => !IsCarrying)
            .Subscribe(inputValue => isFacingLeft.Value = inputValue < 0f);

        inputProvider
            .MoveInput
            // 地面についているかまたはジャンプ中の時
            .Where(_ => onGround.Value || isJumpingUp.Value)
            .Where(input => IsWalking.Value)
            .Where(_ => IsCarrying)
            .Subscribe(inputValue => isCarryingLeft.Value = inputValue < 0f);

        // ジャンプ開始判定
        inputProvider
            .OnJumpInput
            .Where(_ => onGround.Value && !isPushing.Value)
            .Where(_ => !isJumpingUp.Value && !isJumpingDown.Value)
            .Subscribe(_ => JumpFlow().Forget())
            .AddTo(this);

        // 接地判定
        Observable.EveryFixedUpdate()
            .Select(_ => (
                Physics2D.Linecast(
                    leftGroundPoint.position + transform.up * 0.1f,
                    leftGroundPoint.position - transform.up * 0.1f,
                    groundLayer),
                Physics2D.Linecast(
                    rightGroundPoint.position + transform.up * 0.1f,
                    rightGroundPoint.position - transform.up * 0.1f,
                    groundLayer))
            )
            .Subscribe(tuple => onGround.Value = tuple.Item1 || tuple.Item2)
            .AddTo(this);

        // 運搬開始判定
        inputProvider
            .CarryInput
            .Where(carry => carry)
            .Where(_ => !IsCarrying)
            .Where(_ => onGround.Value && !isPushing.Value)
            .Where(_ => !isJumpingUp.Value && !isJumpingDown.Value)
            .Where(_ => !Mathf.Approximately(inputProvider.MoveInput.Value, 0f))
            .Select(_ => inputProvider.MoveInput.Value)
            .Select(moveInput => moveInput < 0f
                ? (pushPos: leftPushPoint.position, lineVec: Vector3.left)
                : (pushPos: rightPushPoint.position, lineVec: Vector3.right))
            .Select(tuple => Physics2D.Linecast(
                tuple.pushPos + (tuple.lineVec * 0.1f),
                tuple.pushPos + (-tuple.lineVec * 0.1f)))
            .Where(hit => hit.collider != null && hit.collider.CompareTag("Carriable"))
            .Subscribe(hit =>
            {
                carryTarget = hit.transform;
                // carryTarget.parent = transform;
                if (carryTarget.gameObject.GetComponent<Rigidbody2D>())
                {
                    return;
                }

                carryRigidbody = carryTarget.gameObject.AddComponent<Rigidbody2D>();
                var leftCarry = inputProvider.MoveInput.Value < 0f;
                if (leftCarry == IsFacingLeft.Value)
                {
                    isPushing.Value = true;
                }
                else
                {
                    isPulling.Value = true;
                }
            })
            .AddTo(this);

        // 運搬終了判定1
        inputProvider
            .CarryInput
            .Where(carry => !carry)
            .Where(_ => IsCarrying)
            .Subscribe(_ =>
            {
                // carryTarget.parent = null;
                if (Mathf.Abs(carryRigidbody.velocity.y) < 0.1f &&
                    IsZeroFloat(carryTarget.eulerAngles.z, 0.1f))
                {
                    Destroy(carryTarget.GetComponent<Rigidbody2D>());
                }

                carryTarget = null;
                isPushing.Value = false;
                isPulling.Value = false;
            })
            .AddTo(this);

        // 落下することで終了
        Observable.EveryFixedUpdate()
            .Where(_ => IsCarrying)
            .Where(_ => Mathf.Abs(carryRigidbody.velocity.y) > 0.1f)
            .Subscribe(_ =>
            {
                carryTarget = null;
                isPushing.Value = false;
                isPulling.Value = false;
            });

        // 運搬方向を変える処理
        IsCarryingLeft
            .Where(_ => IsCarrying)
            .Subscribe(_ =>
            {
                var leftCarry = inputProvider.MoveInput.Value < 0f;
                if (leftCarry == IsFacingLeft.Value)
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
    }

    private async UniTask JumpFlow()
    {
        isJumpingUp.Value = true;
        rb.AddForce(new Vector2(0f, jumpImpulse), ForceMode2D.Impulse);

        await UniTask.WaitWhile(() => rb.velocity.y > 0f);

        isJumpingUp.Value = false;
        isJumpingDown.Value = true;

        // Stageレイヤーで接地判定
        await OnGround
            .Where(onGround => onGround)
            .Take(1)
            .ToUniTask();

        // y軸方向の速さが0になったらジャンプを終了する
        // await UniTask.WaitUntil(() => Mathf.Approximately(rb.velocity.y, 0f));

        isJumpingDown.Value = false;
    }

    private static bool IsZeroFloat(float value, float diff = 0.1f)
    {
        return Mathf.Abs(value) < diff;
    }
}