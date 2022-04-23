using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class SpiderFaller : MonoBehaviour
{
    // [SerializeField] private Vector3 waitPos;
    // [SerializeField] private Vector3 targetPos;
    [SerializeField] private float fallHeight = 7.75f;
    [SerializeField] private float fallDuration = 1f;
    [SerializeField] private float stayDuration = 0.5f;
    [SerializeField] private float returnDuration = 3f;
    [SerializeField] private bool autoFall = false;

    private CancellationToken token;

    private Vector3 waitPos;

    private void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(col => col.CompareTag("PlayerCollider"))
            .Select(col => col.transform.parent.GetComponent<IDieable>())
            .Subscribe(dieable =>
            {
                Debug.Log("TryKill");
                dieable.TryKill(DeadReason.SpiderFall);
            });

        waitPos = transform.position;

        token = this.GetCancellationTokenOnDestroy();
        if (autoFall)
        {
            MoveFlow().Forget();
        }
    }

    private async UniTask MoveFlow()
    {
        while (!token.IsCancellationRequested)
        {
            await FallFlow();
            await UniTask.Delay(TimeSpan.FromSeconds(5f), cancellationToken: token);
        }
    }

    public async UniTask FallFlow()
    {
        // transform.position = waitPos;
        await transform.DOMoveY(-fallHeight, fallDuration)
            .SetRelative()
            .SetEase(Ease.InQuad);

        if (stayDuration > 0f)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(stayDuration), cancellationToken: token);
        }

        await transform.DOMove(waitPos, returnDuration).SetEase(Ease.Linear);
    }
}