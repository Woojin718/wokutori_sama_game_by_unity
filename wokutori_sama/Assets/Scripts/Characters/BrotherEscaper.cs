using UniRx;
using UniRx.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BrotherEscaper : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3.2f;
    [SerializeField] private Collider2D detectArea;
    [SerializeField] private Collider2D escapeArea;
    [SerializeField] private Animator _animator;

    private PlayerCore player;
    [ReadOnly] [SerializeField] private bool isEscaping = false;

    private Rigidbody2D rb;
    private SpriteRenderer _renderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _renderer = _animator.GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCore>();

        detectArea
            .gameObject
            .OnTriggerEnter2DAsObservable()
            .Where(col => col.CompareTag("PlayerCollider"))
            .Subscribe(_ => isEscaping = true);

        escapeArea
            .gameObject
            .OnTriggerExit2DAsObservable()
            .Where(col => col.CompareTag("PlayerCollider"))
            .Subscribe(_ => isEscaping = false);

        Observable.EveryFixedUpdate()
            .Subscribe(_ =>
            {
                if (isEscaping)
                {
                    var velocity = rb.velocity;
                    var direction = (player.transform.position.x - transform.position.x) < 0 ? 1 : -1;
                    _renderer.flipX = direction > 0;
                    rb.velocity = new Vector2(walkSpeed * direction, velocity.y);
                    _animator.Play("Walk");
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    _animator.Play("Idle");
                }
            })
            .AddTo(this);
    }
}