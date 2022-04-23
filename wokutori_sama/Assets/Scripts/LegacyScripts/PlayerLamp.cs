using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PlayerLamp : MonoBehaviour
{
    [SerializeField] private PlayerMover playerMover;
    [SerializeField] private GameObject childLamp;
    [SerializeField] private bool useLamp = true;

    private void Start()
    {
        if (!useLamp)
        {
            childLamp.SetActive(false);
            return;
        }

        // playerMover
        //     .IsFacingLeft
        //     .Subscribe(isFacingLeft => transform.localScale = new Vector3(isFacingLeft ? 1f : -1f, 1f, 1f))
        //     .AddTo(this);
        //
        // this.UpdateAsObservable()
        //     .Select(_ => !playerMover.IsPushing.Value && playerMover.OnGround.Value)
        //     .Subscribe(isOn => childLamp.SetActive(isOn))
        //     .AddTo(this);
    }
}