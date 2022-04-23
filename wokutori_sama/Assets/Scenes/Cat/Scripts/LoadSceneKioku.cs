using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class LoadSceneKioku : MonoBehaviour
{
    public string TitleScene;
    public KiokuPanelScript titleEnter;

    [SerializeField] private Material postEffectMaterial;
    [SerializeField] private float transitionTime = 2f;

    private readonly int _progressID = Shader.PropertyToID("_Progress");


    // Start is called before the first frame update
    void Start()
    {
        titleEnter.OnTitleEnter
            //「_」は「破棄」、何もないことを表す。
            //.Subscribe(_ => SceneManager.LoadScene(TitleScene))
            .Subscribe(_ => LoadEvent().Forget())
            .AddTo(this);
    }

    async UniTask LoadEvent()
    {
        //await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
        await DOTween.To(
            () => 0.99f,
            value => postEffectMaterial.SetFloat(_progressID, value),
            0.01f,
            transitionTime
        );

        SceneManager.LoadScene(TitleScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
