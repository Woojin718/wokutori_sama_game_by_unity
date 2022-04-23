using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class KaonashiCore : MonoBehaviour
{
    [SerializeField] private string lampTag = "Lamp";
    [SerializeField] private Collider2D touchPlayerCollider;

    private readonly Subject<Transform> eatSubject = new Subject<Transform>();
    public IObservable<Transform> OnEat => eatSubject;

    [ReadOnly] [SerializeField] private BoolReactiveProperty isInsideLamp = new BoolReactiveProperty(false);
    public IReadOnlyReactiveProperty<bool> IsInsideLamp => isInsideLamp;

    private Light2D lamp;
    private float lampRadius;

    private void Start()
    {
        lamp = GameObject.FindGameObjectWithTag(lampTag).GetComponent<Light2D>();
        lampRadius = lamp.pointLightOuterRadius;

        this.OnCollisionEnter2DAsObservable()
            .Select(col => col.rigidbody)
            .Where(rb => rb != null)
            .Select(rb => rb.GetComponent<IDieable>())
            .Where(dieable => dieable != null)
            .Subscribe(dieable =>
            {
                var killed = dieable.TryKill(DeadReason.KaonashiEat);
                if (killed)
                {
                    Debug.Log("Kaonashi killed player.");
                    eatSubject.OnNext(dieable.transform);
                }
            });

        Observable.EveryFixedUpdate()
            .Subscribe(_ => isInsideLamp.Value =
                (lamp.transform.position - transform.position).magnitude < lampRadius &&
                lamp.intensity > 0.5f)
            .AddTo(this);

        IsInsideLamp
            .Subscribe(isInsideLamp => touchPlayerCollider.isTrigger = isInsideLamp)
            .AddTo(this);
    }
}