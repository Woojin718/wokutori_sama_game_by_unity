using System;
using UniRx;
using UnityEngine;

public class PlayerInputProvider : MonoBehaviour
{
    [ReadOnly] [SerializeField] private FloatReactiveProperty moveInput = new FloatReactiveProperty(0f);
    public IReadOnlyReactiveProperty<float> MoveInput => moveInput;

    private readonly Subject<Unit> jumpInputSubject = new Subject<Unit>();
    public IObservable<Unit> OnJumpInput => jumpInputSubject;

    [ReadOnly] [SerializeField] private BoolReactiveProperty carryInput = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> CarryInput => carryInput;

    // [Header("テスト用")] [ReadOnly] [SerializeField]
    // private BoolReactiveProperty lampIsOn = new BoolReactiveProperty(false);
    // public IReadOnlyReactiveProperty<bool> LampIsOn => lampIsOn;

    private readonly Subject<Unit> lampSwitchSubject = new Subject<Unit>();
    public IObservable<Unit> OnLampSwitch => lampSwitchSubject;

    private void Start()
    {
        Observable.EveryFixedUpdate()
            .Select(_ => Input.GetAxis("Horizontal"))
            .SubscribeWithState(moveInput,
                (inputValue, inputProp) => inputProp.SetValueAndForceNotify(inputValue))
            .AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.UpArrow))
            .Subscribe(_ => jumpInputSubject.OnNext(Unit.Default))
            .AddTo(this);

        Observable.EveryUpdate()
            .Select(_ => Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
            .Subscribe(pushable => carryInput.SetValueAndForceNotify(pushable))
            .AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .ThrottleFirst(TimeSpan.FromSeconds(1f))
            // .Subscribe(_ => lampIsOn.Value = !lampIsOn.Value)
            .Subscribe(_ => lampSwitchSubject.OnNext(Unit.Default))
            .AddTo(this);
    }
}