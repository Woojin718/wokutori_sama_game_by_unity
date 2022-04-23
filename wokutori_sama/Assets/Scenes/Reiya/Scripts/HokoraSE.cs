using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;
public class HokoraSE : MonoBehaviour
{
    public Transform player;
    public Transform ToriiCollider;

    float stopBGM;
    public float startSE;
    public bool BGMBool = true;
    public bool SEBool = false;


    // Start is called before the first frame update
    void Start()
    {
        stopBGM = Vector2.Distance(transform.position, ToriiCollider.position);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < stopBGM && BGMBool == true)
        {
            BGMSwitcher.CrossFade(BGMPath.WIND, 10);

            //BGMManager.Instance.FadeOut(BGMPath.SEMI, 5);
            Debug.Log("BGMStop");
            BGMBool = false;

        }

        else if (distance >= stopBGM && BGMBool == false)
        {
            BGMSwitcher.CrossFade(BGMPath.SEMI, 5);

            //BGMManager.Instance.FadeIn(BGMPath.SEMI);
            Debug.Log("BGMStart");
            BGMBool = true;
        }


        /*if (distance < startSE && SEBool == true)
        {
            BGMManager.Instance.FadeIn(BGMPath.WIND);
            Debug.Log("SEStart");
            SEBool = false;
        }

        else if (distance >= startSE && SEBool == false && BGMBool == false)
        {
            BGMManager.Instance.FadeOut(BGMPath.WIND, 5);
            Debug.Log("SEStop");
            SEBool = true;
        }*/
    }


}
