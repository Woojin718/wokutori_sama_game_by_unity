using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SaveMessage : MonoBehaviour
{
    private Text messageText;

    private bool displayNow = false;

    private Sequence _sequence;

    private void Start()
    {
        messageText = GetComponent<Text>();
        SetTextAlpha(messageText, 0f);

        var token = this.GetCancellationTokenOnDestroy();

        SaveManager.Instance
            .CurrentSaveIndex
            .SkipLatestValueOnSubscribe()
            .Subscribe(_ => { DisplayMessageFlow(token).Forget(); })
            .AddTo(this);
    }

    private async UniTask DisplayMessageFlow(CancellationToken token)
    {
        if (displayNow)
        {
            _sequence.Complete();
        }

        displayNow = true;
        SetTextAlpha(messageText, 1f);

        await UniTask.Delay(TimeSpan.FromSeconds(5f), cancellationToken: token);

        _sequence = DOTween.Sequence();
        _sequence.Append(messageText.DOFade(0f, 2f));
        await _sequence.WithCancellation(token);

        displayNow = false;
    }

    private static void SetTextAlpha(Text text, float a)
    {
        var color = text.color;
        color.a = a;
        text.color = color;
    }
}