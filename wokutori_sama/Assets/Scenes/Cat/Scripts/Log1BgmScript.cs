using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class Log1BgmScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BGMManager.Instance.Play(BGMPath.SEMI);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
