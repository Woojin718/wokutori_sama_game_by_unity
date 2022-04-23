using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class GameOverView : MonoBehaviour
{
    private void Start()
    {
        var canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;

        GameManager.Instance
            .CurrentState
            .Where(state => state == GameState.Failure)
            .Delay(TimeSpan.FromSeconds(2f))
            .Subscribe(_ => canvasGroup.DOFade(1f, 2f))
            .AddTo(this);
    }
}