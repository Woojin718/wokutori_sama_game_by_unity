using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(KaonashiCore))]
[RequireComponent(typeof(Rigidbody2D))]
public class KaonashiMover : MonoBehaviour
{
    [SerializeField] private bool isLeftStart = true;
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float chaseSpeed = 3.1f;
    [SerializeField] private Transform rayOrigin;

    private readonly ReactiveProperty<KaonashiState> currentState =
        new ReactiveProperty<KaonashiState>(KaonashiState.Patrol);
    public IReadOnlyReactiveProperty<KaonashiState> CurrentState => currentState;

    [ReadOnly] [SerializeField] private Vector2ReactiveProperty currentDirection = new Vector2ReactiveProperty();
    public IReadOnlyReactiveProperty<Vector2> CurrentDirection => currentDirection;

    private KaonashiCore core;
    private Rigidbody2D rb;
    private PlayerCore player;

    private bool canDetect = true;

    private void Start()
    {
        core = GetComponent<KaonashiCore>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>();
        currentDirection.Value = isLeftStart ? Vector2.left : Vector2.right;

        this.OnCollisionEnter2DAsObservable()
            .Where(col => LayerMask.LayerToName(col.gameObject.layer) == "Field")
            .Subscribe(_ =>
            {
                currentDirection.Value = -currentDirection.Value;
                currentState.Value = KaonashiState.Patrol;
            })
            .AddTo(this);

        Observable.EveryFixedUpdate()
            .Where(_ => currentState.Value == KaonashiState.Patrol)
            .Subscribe(_ => rb.velocity = walkSpeed * currentDirection.Value)
            .AddTo(this);

        Observable.EveryFixedUpdate()
            .Where(_ => currentState.Value == KaonashiState.Chase)
            .Subscribe(_ => rb.velocity = chaseSpeed * currentDirection.Value)
            .AddTo(this);

        core.OnEat
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

        this.OnTriggerStay2DAsObservable()
            .Where(_ => CurrentState.Value == KaonashiState.Patrol)
            .Where(_ => !player.LampIsOn.Value)
            .Where(col => col.CompareTag("PlayerCollider"))
            .Subscribe(_ =>
            {
                var direction = (player.transform.position - rayOrigin.position);
                var hits = new RaycastHit2D[5];
                Physics2D.RaycastNonAlloc(rayOrigin.position, direction, hits, direction.magnitude);

                foreach (var hit in hits)
                {
                    // コライダー情報がない場合、ヒットしていないものとして次のコライダーへ。
                    if (hit.collider == null) continue;
                    // ヒットしたコライダーの親が自身と一致する場合は、ヒットしていないものとして次の判定へ。
                    if (hit.transform.root == transform) continue;

                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Field"))
                    {
                        return;
                    }
                }

                currentDirection.Value =
                    (player.transform.position.x - transform.position.x) < 0f ? Vector2.left : Vector2.right;

                Debug.Log("Detect player.");
                currentState.Value = KaonashiState.Chase;
            });
    }

    private async UniTask DelayDetectFlow(CancellationToken token)
    {
        canDetect = false;
        await UniTask.Delay(TimeSpan.FromSeconds(5f), cancellationToken: token);
        canDetect = true;
    }
}