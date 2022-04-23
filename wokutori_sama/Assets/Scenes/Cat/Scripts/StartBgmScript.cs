using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class StartBgmScript : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        BGMManager.Instance.Play(BGMPath.WOKU_TITLE);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
