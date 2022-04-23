using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using UniRx;

public class LoadSceneCollider : MonoBehaviour
{
    //ステージの端に当たったことを通知するサブジェクト（UniRX使ってるやつに伝達する機能）
    public Subject<Unit> OnLoadSceneEnter = new Subject<Unit>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("!!!");
        if (other.gameObject.tag == "PlayerCollider")
        {
            Debug.Log("!");
            OnLoadSceneEnter.OnNext(Unit.Default);
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{

    //    if (collision.gameObject.tag == "Player")
    //    {
    //        OnLoadSceneEnter.OnNext(Unit.Default);
    //    }
    //}
}
