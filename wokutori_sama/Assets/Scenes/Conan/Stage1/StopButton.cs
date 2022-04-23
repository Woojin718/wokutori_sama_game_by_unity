using System.Runtime.CompilerServices;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopButton : MonoBehaviour
{
    public GameObject startButton;
    public GameObject stopButton;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void stopOn() {
        
        Debug.Log("Stop");
        startButton.gameObject.SetActive(true);
        Time.timeScale= 0.0f;
        stopButton.gameObject.SetActive(false);
        
    }
    public void starOn() {
        startButton.gameObject.SetActive(false);
        Time.timeScale = 1.0f;
        stopButton.gameObject.SetActive(true);
        Debug.Log("Start");
    }
}
