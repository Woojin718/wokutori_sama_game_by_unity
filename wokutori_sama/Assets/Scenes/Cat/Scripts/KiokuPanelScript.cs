using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using UniRx;


public class KiokuPanelScript : MonoBehaviour
{
    
    public GameObject[] mText;
    //点滅する方
    public GameObject[] mText_t;
    public GameObject[] sankakus;

    public int select = 0;

    public bool PanelOn = false;
    public bool EnterLock = false;


    public Image[] Panel;

    public Text[] PanelTexts1;
    public Text[] PanelTexts2;
    public Text[] PanelTexts3;
    public Text[] PanelTexts4;
    public Text[] PanelTexts5;
    public Text[] PanelTexts6;
    public Text[] PanelTexts7;

    public GameObject BackText;
    //public Text BackSankaku;

    public float FadeTime = 1.0f;
    public float DelayTime = 0.5f;


    //フェードが全て終わったことを通知するサブジェクト（UniRX使ってるやつに伝達する機能）
    public Subject<Unit> OnTitleEnter = new Subject<Unit>();

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
        PanelOn = this.GetComponent<PanelBool>().PanelOn;
        EnterLock = this.GetComponent<PanelBool>().EnterLock;

        if (PanelOn == false)
        {

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

            }
        }
        


        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (EnterLock == false)
            {
                if (select == 0)
                {
                    OnTitleEnter.OnNext(Unit.Default);
                }
                if (select <= 7 && select > 0)
                {
                    if (PanelOn == false)
                    {
                        Debug.Log("フェードイン、キーロック");
                        Panel[select - 1].DOFade(0.9f, FadeTime);
                        var token = this.GetCancellationTokenOnDestroy();
                        TextFlow(token).Forget();

                    }
                    if (PanelOn == true)
                    {

                        Panel[select - 1].DOFade(0f, 2.0f);
                        if (select == 1)
                        {
                            foreach (Text text in PanelTexts1)
                            {
                                text.DOFade(0f, 1.0f);
                            }

                            Debug.Log("フェードアウト、キーロック解除");
                        }
                        if (select == 2)
                        {
                            foreach (Text text in PanelTexts2)
                            {
                                text.DOFade(0f, 1.0f);
                            }
                        }
                        if (select == 3)
                        {
                            foreach (Text text in PanelTexts3)
                            {
                                text.DOFade(0f, 1.0f);
                            }
                        }
                        if (select == 4)
                        {
                            foreach (Text text in PanelTexts4)
                            {
                                text.DOFade(0f, 1.0f);
                            }
                        }
                        if (select == 5)
                        {
                            foreach (Text text in PanelTexts5)
                            {
                                text.DOFade(0f, 1.0f);
                            }
                        }
                        if (select == 6)
                        {
                            foreach (Text text in PanelTexts6)
                            {
                                text.DOFade(0f, 1.0f);
                            }
                        }
                        if (select == 7)
                        {
                            foreach (Text text in PanelTexts7)
                            {
                                text.DOFade(0f, 1.0f);
                            }
                        }
                        BackText.SetActive(false);


                    }

                }
            }
            if (EnterLock == true)
            {

                Debug.Log("エンターはロックされています");
            }



        }
    }

    private async UniTask TextFlow(CancellationToken token)
    {
        if (select == 1)
        {
            //配列の中から要素を一つずつ取り出して
            foreach (Text text in PanelTexts1)
            {
                //一行ずつフェードインしていく(アルファの目標,フェード時間)
                text.DOFade(1.0f, FadeTime);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayTime), cancellationToken: token);
            }
        }
        if (select == 2)
        {
            //配列の中から要素を一つずつ取り出して
            foreach (Text text in PanelTexts2)
            {
                //一行ずつフェードインしていく(アルファの目標,フェード時間)
                text.DOFade(1.0f, FadeTime);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayTime), cancellationToken: token);
            }
        }
        if (select == 3)
        {
            //配列の中から要素を一つずつ取り出して
            foreach (Text text in PanelTexts3)
            {
                //一行ずつフェードインしていく(アルファの目標,フェード時間)
                text.DOFade(1.0f, FadeTime);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayTime), cancellationToken: token);
            }
        }
        if (select == 4)
        {
            //配列の中から要素を一つずつ取り出して
            foreach (Text text in PanelTexts4)
            {
                //一行ずつフェードインしていく(アルファの目標,フェード時間)
                text.DOFade(1.0f, FadeTime);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayTime), cancellationToken: token);
            }
        }
        if (select == 5)
        {
            //配列の中から要素を一つずつ取り出して
            foreach (Text text in PanelTexts5)
            {
                //一行ずつフェードインしていく(アルファの目標,フェード時間)
                text.DOFade(1.0f, FadeTime);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayTime), cancellationToken: token);
            }
        }
        if (select == 6)
        {
            //配列の中から要素を一つずつ取り出して
            foreach (Text text in PanelTexts6)
            {
                //一行ずつフェードインしていく(アルファの目標,フェード時間)
                text.DOFade(1.0f, FadeTime);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayTime), cancellationToken: token);
            }
        }
        if (select == 7)
        {
            //配列の中から要素を一つずつ取り出して
            foreach (Text text in PanelTexts7)
            {
                //一行ずつフェードインしていく(アルファの目標,フェード時間)
                text.DOFade(1.0f, FadeTime);
                await UniTask.Delay(TimeSpan.FromSeconds(DelayTime), cancellationToken: token);
            }
        }

        //1秒待つ
        await UniTask.Delay(TimeSpan.FromSeconds(2.0f), cancellationToken: token);
        BackText.SetActive(true);

    }

}
