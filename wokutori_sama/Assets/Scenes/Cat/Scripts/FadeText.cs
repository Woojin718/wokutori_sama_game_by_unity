using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using UniRx;

public class FadeText : MonoBehaviour
{
    public Text[] texts;
    public float FadeTime = 1.0f;
    public float DelayTime = 0.5f;
    public float FirstDelay = 1.0f;
    public float LastDelay = 0f;

    //フェードが全て終わったことを通知するサブジェクト（UniRX使ってるやつに伝達する機能）
    public Subject<Unit> OnFadeEnd = new Subject<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        var token = this.GetCancellationTokenOnDestroy();
        TextFlow(token).Forget();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async UniTask TextFlow(CancellationToken token)
    {
        //1秒待つ
        await UniTask.Delay(TimeSpan.FromSeconds(FirstDelay),cancellationToken: token);

        //配列の中から要素を一つずつ取り出して
        foreach(Text text in texts)
        {
            //一行ずつフェードインしていく(アルファの目標,フェード時間)
            text.DOFade(1.0f, FadeTime);
            await UniTask.Delay(TimeSpan.FromSeconds(DelayTime),cancellationToken: token);
        }

        //待つ
        await UniTask.Delay(TimeSpan.FromSeconds(LastDelay), cancellationToken: token);

        OnFadeEnd.OnNext(Unit.Default);
    }
}
