using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using UniRx;

public class KiokuScript : MonoBehaviour
{
    public GameObject[] mText;

    //点滅する方
    public GameObject[] mText_t;

    public GameObject[] sankakus;

    public Text[] PanelTexts1;
    public Text[] PanelTexts2;
    public Text[] PanelTexts3;
    public Text[] PanelTexts4;
    public Text[] PanelTexts5;
    public Text[] PanelTexts6;
    public Text[] PanelTexts7;



    public int select = 0;

    public float FadeTime = 1.0f;
    public float DelayTime = 0.5f;
    public Image[] panel;

    //フェードが全て終わったことを通知するサブジェクト（UniRX使ってるやつに伝達する機能）
    public Subject<Unit> OnTitleEnter = new Subject<Unit>();

    public bool TextFlowing = false;

    // Start is called before the first frame update
    void Start()
    {
        select = 1;
        foreach (GameObject mt in mText)
        {
            mt.SetActive(true);
        }
        foreach (GameObject mtt in mText_t)
        {
            mtt.SetActive(false);
        }
        foreach (GameObject s in sankakus)
        {
            s.SetActive(false);
        }
        mText[select].SetActive(false);
        sankakus[select].SetActive(true);
        mText_t[select].SetActive(true);

        
        
    }

    // Update is called once per frame
    void Update()
    {
        // 上に移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (select > 0)
            {
                select--;
            }
            foreach (GameObject mt in mText)
            {
                mt.SetActive(true);
            }
            foreach (GameObject mtt in mText_t)
            {
                mtt.SetActive(false);
            }
            foreach (GameObject s in sankakus)
            {
                s.SetActive(false);
            }
            mText[select].SetActive(false);
            mText_t[select].SetActive(true);
            sankakus[select].SetActive(true);
            
        }

        // 下に移動
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(TextFlowing == false)
            {
                if (select < 7 && select >= 0)
                {
                    select++;
                    foreach (GameObject mt in mText)
                    {
                        mt.SetActive(true);
                    }
                    foreach (GameObject mtt in mText_t)
                    {
                        mtt.SetActive(false);
                    }
                    foreach (GameObject s in sankakus)
                    {
                        s.SetActive(false);
                    }
                    mText[select].SetActive(false);
                    mText_t[select].SetActive(true);
                    sankakus[select].SetActive(true);
                }


                if (select < -1)
                {
                    panel[select + 9].DOFade(0f, 1.0f);
                    var token = this.GetCancellationTokenOnDestroy();

                    foreach (Text text in PanelTexts1)
                    {
                        
                        text.DOFade(0f, 1.0f);

                    }

                    TextOut(token).Forget();

                }
            }
            

        }




        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (select == 0)
            {
                OnTitleEnter.OnNext(Unit.Default);
            }
            else if (select > 0)
            {
                TextFlowing = true;
                panel[select-1].DOFade(0.9f, FadeTime);
                var token = this.GetCancellationTokenOnDestroy();
                TextFlow(token).Forget();
                select = select - 10;
            }
            
        }

    }
    private async UniTask TextFlow(CancellationToken token)
    {
        //1秒待つ
        await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: token);

        foreach (Text text in PanelTexts1)
        {
            //一行ずつフェードインしていく(アルファの目標,フェード時間)
            text.DOFade(1.0f, FadeTime);
            await UniTask.Delay(TimeSpan.FromSeconds(DelayTime), cancellationToken: token);
        }
        
        await UniTask.Delay(TimeSpan.FromSeconds(5.0f), cancellationToken: token);
        TextFlowing = false;
    }

    private async UniTask TextOut(CancellationToken token)
    {
        //1秒待つ
        await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: token);

        select = select + 10;
    }
}
