using DG.Tweening;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(KaonashiCore))]
[RequireComponent(typeof(KaonashiMover))]
public class KaonashiAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer kaonashiRenderer;

    private Animator animator;
    private KaonashiCore kaonashiCore;
    private KaonashiMover kaonashiMover;

    private void Start()
    {
        animator = kaonashiRenderer.GetComponent<Animator>();
        kaonashiCore = GetComponent<KaonashiCore>();
        kaonashiMover = GetComponent<KaonashiMover>();

        animator.Play("Walk");

        kaonashiMover
            .CurrentDirection
            .Subscribe(direction => kaonashiRenderer.flipX = direction == Vector2.right)
            .AddTo(this);

        kaonashiCore
            .OnEat
            .Subscribe(Eat)
            .AddTo(this);

        kaonashiMover
            .CurrentState
            .Subscribe(state =>
            {
                switch (state)
                {
                    case KaonashiState.Patrol:
                        animator.speed = 1f;
                        break;
                    case KaonashiState.Chase:
                        animator.speed = 2f;
                        break;
                }
            })
            .AddTo(this);
    }

    private void Eat(Transform eatTarget)
    {
        animator.Play("Eat");
        var targetPos = eatTarget.position;
        transform.DOMove(targetPos, 1.4f);
    }
}