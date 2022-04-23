using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(PlayerCore))]
[RequireComponent(typeof(PlayerMover2))]
[RequireComponent(typeof(PlayerInputProvider))]
public class PlayerDrawer : MonoBehaviour
{
    [SerializeField] private bool flipWhenLeft = false;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator lampAnimator;
    [SerializeField] private Light2D light2D;
    [SerializeField] private SpriteMask mask;

    private PlayerCore playerCore;
    private PlayerMover2 playerMover;
    private PlayerInputProvider inputProvider;
    private SpriteRenderer playerRenderer;
    private SpriteRenderer lampRenderer;

    private string GetOnOrOff => playerCore.LampIsOn.Value ? "On" : "Off";

    private float lampIntensity;

    private bool isLampSwitching;

    private bool isExecuteKindleFlow = false;

    private const string JumpUp = "JumpUp";
    private const string JumpUpStop = "JumpUpStop";
    private const string JumpDown = "JumpDown";
    private const string JumpDownStop = "JumpDownStop";
    private const string Push = "Push";
    private const string Pull = "Pull";
    private const string Walk = "Walk";
    private const string PreparePush = "PreparePush";
    private const string PreparePull = "PreparePull";
    private const string Idle = "Idle";
    private const string Kindle = "Kindle";

    private void Start()
    {
        playerCore = GetComponent<PlayerCore>();
        playerMover = GetComponent<PlayerMover2>();
        inputProvider = GetComponent<PlayerInputProvider>();
        playerRenderer = playerAnimator.GetComponent<SpriteRenderer>();
        lampRenderer = lampAnimator.GetComponent<SpriteRenderer>();

        lampIntensity = light2D.intensity;

        // inputProvider
        playerCore
            .LampIsOn
            .Where(_ => !playerCore.IsDead.Value)
            .Subscribe(lampIsOn =>
            {
                if (!lampIsOn)
                {
                    isLampSwitching = true;
                    DOTween.To(
                        () => light2D.intensity,
                        (x) => light2D.intensity = x,
                        0f,
                        1f
                    ).OnComplete(() =>
                    {
                        isLampSwitching = false;
                        mask.enabled = false;
                    });
                }
                else
                {
                    isLampSwitching = true;
                    DOTween.To(
                        () => light2D.intensity,
                        (x) => light2D.intensity = x,
                        lampIntensity,
                        1f
                    ).OnComplete(() =>
                    {
                        isLampSwitching = false;
                        mask.enabled = true;
                    });
                }
            })
            .AddTo(this);

        playerMover
            .IsFacingLeft
            .Subscribe(isFacingLeft =>
            {
                var isFlip = flipWhenLeft ? isFacingLeft : !isFacingLeft;
                playerRenderer.flipX = isFlip;
                lampRenderer.flipX = isFlip;
            })
            .AddTo(this);

        playerCore.OnPlayerDied
            .Subscribe(_ =>
            {
                DOTween.To(
                    () => light2D.intensity,
                    (x) => light2D.intensity = x,
                    0f,
                    1f
                ).OnComplete(() => mask.enabled = false);

                lampRenderer.enabled = false;
                playerAnimator.Play("Die");
            })
            .AddTo(this);

        var token = this.GetCancellationTokenOnDestroy();

        playerCore.IsSaving
            .Where(isSaving => isSaving)
            .Subscribe(_ => KindleFlow(token).Forget())
            .AddTo(this);

        playerCore.CanMove
            .Where(canMove => !canMove)
            .Subscribe(_ => PlayAnimation(Idle))
            .AddTo(this);
    }

    private void Update()
    {
        if (playerCore.IsDead.Value || !playerCore.CanMove.Value)
        {
            return;
        }

        if (playerMover.IsMoving)
        {
            if (playerMover.IsJumping)
            {
                if (playerMover.IsJumpingUp.Value)
                {
                    if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(JumpUp)
                        && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(JumpUpStop)
                    )
                    {
                        PlayAnimation(JumpUp);
                    }
                }
                else
                {
                    if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(JumpDown)
                        && !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(JumpDownStop)
                    )
                    {
                        PlayAnimation(JumpDown);
                    }
                }
            }
            else if (playerMover.IsCarrying)
            {
                if (playerMover.IsPushing.Value)
                {
                    PlayAnimation(Push);
                }
                else
                {
                    PlayAnimation(Pull);
                }
            }
            else if (playerMover.IsWalking.Value)
            {
                PlayAnimation(Walk);
            }
        }
        else
        {
            if (playerMover.IsCarrying)
            {
                if (playerMover.IsPushing.Value)
                {
                    PlayAnimation(PreparePush);
                }
                else
                {
                    PlayAnimation(PreparePull);
                }
            }
            else
            {
                PlayAnimation(Idle);
            }
        }
    }

    private void PlayAnimation(string stateName)
    {
        playerAnimator.Play(stateName);
        var lampStateName = GetOnOrOff + stateName;
        lampAnimator.Play(lampStateName);
    }

    private async UniTask KindleFlow(CancellationToken token)
    {
        if (isExecuteKindleFlow)
        {
            return;
        }

        isExecuteKindleFlow = true;

        PlayAnimation(Kindle);
        await UniTask.DelayFrame(1, cancellationToken: token);

        await UniTask.WaitUntil(() =>
        {
            var stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.normalizedTime >= 0.4f;
        }, cancellationToken: token);

        playerCore.OnKindled();

        await UniTask.WaitUntil(() =>
        {
            var stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.normalizedTime >= 1f;
        }, cancellationToken: token);

        playerCore.EndSave();

        isExecuteKindleFlow = false;
    }
}