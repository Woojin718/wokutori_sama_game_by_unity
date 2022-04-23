using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(PlayerInputProvider))]
public class PlayerCore : MonoBehaviour, IDieable
{
    private readonly Subject<DeadReason> playerDiedSubject = new Subject<DeadReason>();
    public IObservable<DeadReason> OnPlayerDied => playerDiedSubject;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isDead = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsDead => isDead;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isSaving = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsSaving => isSaving;

    [ReadOnly] [SerializeField] private BoolReactiveProperty canMove = new BoolReactiveProperty(true);
    public IReadOnlyReactiveProperty<bool> CanMove => canMove;

    [ReadOnly] [SerializeField] private BoolReactiveProperty lampIsOn = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> LampIsOn => lampIsOn;

    public Action OnKindled = null;

    private CinemachineVirtualCamera playerVirtualCamera;
    private PlayerCamera playerCameraComponent;

    private void Start()
    {
        playerDiedSubject
            .Subscribe(_ => isDead.Value = true)
            .AddTo(this);

        playerVirtualCamera = GameObject.FindGameObjectWithTag("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
        playerCameraComponent = playerVirtualCamera.GetComponent<PlayerCamera>();

        GetComponent<PlayerInputProvider>()
            .OnLampSwitch
            .Subscribe(_ => lampIsOn.Value = !lampIsOn.Value)
            .AddTo(this);
    }

    public bool TryKill(DeadReason deadReason)
    {
        if (IsDead.Value)
        {
            return false;
        }

        switch (deadReason)
        {
            case DeadReason.KaonashiEat:
                playerDiedSubject.OnNext(deadReason);
                playerDiedSubject.OnCompleted();
                return true;
            case DeadReason.SpiderFall:
                playerDiedSubject.OnNext(deadReason);
                playerDiedSubject.OnCompleted();
                return true;
        }

        return false;
    }

    public async UniTask StartSaveFlow(Action onKindled)
    {
        canMove.Value = false;

        if (playerVirtualCamera != null)
        {
            playerVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 0f;
            await DOTween.To(
                () => playerVirtualCamera.m_Lens.OrthographicSize,
                s => playerVirtualCamera.m_Lens.OrthographicSize = s,
                playerCameraComponent.ZoomSize,
                2f);
        }

        OnKindled = onKindled;
        isSaving.Value = true;
    }

    public void EndSave()
    {
        playerVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = 1f;
        DOTween.To(
            () => playerVirtualCamera.m_Lens.OrthographicSize,
            s => playerVirtualCamera.m_Lens.OrthographicSize = s,
            playerCameraComponent.NormalSize,
            2f);

        OnKindled = null;
        isSaving.Value = false;

        canMove.Value = true;
    }
}