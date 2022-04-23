using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using UniRx;

public class titleScript : MonoBehaviour
{
    public GameObject startText;
    public GameObject continueText;
    public GameObject memoryText;

    //点滅する方
    public GameObject startT;
    public GameObject continueT;
    public GameObject memoryT;

    public GameObject sankaku1;
    public GameObject sankaku2;
    public GameObject sankaku3;

    public int select = 1;

    //フェードが全て終わったことを通知するサブジェクト（UniRX使ってるやつに伝達する機能）
    public Subject<Unit> OnStartEnter = new Subject<Unit>();
    public Subject<Unit> OnKiokuEnter = new Subject<Unit>();

    // Start is called before the first frame update
    void Start()
    {
        select = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // 上に移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (select > 1)
            {
                select--;
            }
        }

        // 下に移動
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (select < 3)
            {
                select++;
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (select == 1)
            {
                OnStartEnter.OnNext(Unit.Default);
            }
            if (select == 2)
            {
                OnStartEnter.OnNext(Unit.Default);
            }
            if (select == 3)
            {
                OnKiokuEnter.OnNext(Unit.Default);
            }
        }

        //もし1番目なら
        if (select == 1)
        {
            //一番上の三角を表示
            sankaku1.SetActive(true);

            //「はじめから」を点滅させる
            startText.SetActive(false);
            startT.SetActive(true);
            
            //それ以外の表示をオフ
            continueT.SetActive(false);
            continueText.SetActive(true);
            sankaku2.SetActive(false);
        }
        //もし2番目なら
        if (select == 2)
        {
            //「つづきから」を点滅させる
            continueText.SetActive(false);
            continueT.SetActive(true);
            //真ん中の三角を表示
            sankaku2.SetActive(true);
            //それ以外の表示をオフ
            startT.SetActive(false);
            startText.SetActive(true);
            sankaku1.SetActive(false);

            memoryT.SetActive(false);
            memoryText.SetActive(true);
            sankaku3.SetActive(false);
        }
        //もし3番目なら
        if (select == 3)
        {
            //「きおく」を点滅させる
            memoryText.SetActive(false);
            memoryT.SetActive(true);
            //一番下の三角を表示
            sankaku3.SetActive(true);
            //それ以外の表示をオフ
            continueT.SetActive(false);
            continueText.SetActive(true);
            sankaku2.SetActive(false);
        }
    }
}
