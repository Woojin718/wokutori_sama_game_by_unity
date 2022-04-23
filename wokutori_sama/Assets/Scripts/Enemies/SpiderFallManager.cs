using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpiderFallManager : SingletonMonoBehaviour<SpiderFallManager>
{
    [SerializeField] private GameObject spiderPrefab;
    [SerializeField] private int spiderCount;
    [SerializeField] private float xInterval;
    [SerializeField] private float fallDuration;
    [SerializeField] private bool isRandomized = true;

    private CancellationToken token;

    private SpiderFaller[] spiders;

    private void Start()
    {
        token = this.GetCancellationTokenOnDestroy();
        spiders = new SpiderFaller[spiderCount];
        for (var i = 0; i < spiderCount; i++)
        {
            var pos = transform.position;
            pos.x += xInterval * i;
            spiders[i] = Instantiate(spiderPrefab, pos, spiderPrefab.transform.rotation)
                .GetComponent<SpiderFaller>();
        }

        FallSpidersFlow().Forget();
    }

    private async UniTask FallSpidersFlow()
    {
        int currentIndex;
        if (isRandomized)
        {
            currentIndex = Random.Range(0, spiderCount);
        }
        else
        {
            currentIndex = 0;
        }

        while (!token.IsCancellationRequested)
        {
            spiders[currentIndex].FallFlow();
            if (isRandomized)
            {
                currentIndex = Random.Range(0, spiderCount);
            }
            else
            {
                currentIndex = (currentIndex + 1) % spiderCount;
            }

            await UniTask.Delay(TimeSpan.FromSeconds(fallDuration), cancellationToken: token);
        }
    }
}