using UniRx;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private bool _flipWhenLeft = false;
    [SerializeField] private PlayerMover playerMover;

    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsFalling = Animator.StringToHash("IsFalling");
    private static readonly int PreparePush = Animator.StringToHash("PreparePush");
    private static readonly int IsPushing = Animator.StringToHash("IsPushing");
    private static readonly int PreparePull = Animator.StringToHash("PreparePull");
    private static readonly int IsPulling = Animator.StringToHash("IsPulling");

    private void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var animator = GetComponent<Animator>();

        playerMover
            .IsFacingLeft
            .SubscribeWithState(_flipWhenLeft,
                (isFacingLeft, flipWhenLeft) => spriteRenderer.flipX = flipWhenLeft ? isFacingLeft : !isFacingLeft);

        playerMover
            .IsWalking
            .Subscribe(isWalking => animator.SetBool(IsWalking, isWalking));

        playerMover
            .IsJumpingUp
            .SkipLatestValueOnSubscribe()
            .Where(isJumpingUp => isJumpingUp)
            .Subscribe(_ => animator.SetBool(IsJumping, true));

        playerMover
            .IsJumpingDown
            .SkipLatestValueOnSubscribe()
            .Where(isJumpingDown => !isJumpingDown)
            .Subscribe(_ => animator.SetBool(IsJumping, false));

        playerMover
            .OnGround
            .Subscribe(onGround => animator.SetBool(IsFalling, !onGround));

        playerMover
            .IsPushing
            .Subscribe(isPushing =>
            {
                animator.SetBool(PreparePush, isPushing);
                animator.SetBool(IsPushing, isPushing);
            });

        playerMover
            .IsPulling
            .Subscribe(isPulling =>
            {
                animator.SetBool(PreparePull, isPulling);
                animator.SetBool(IsPulling, isPulling);
            });
    }
}