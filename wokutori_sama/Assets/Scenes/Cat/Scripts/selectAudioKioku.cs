using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class selectAudioKioku : MonoBehaviour
{
    public bool PanelOn = false;
    public bool EnterLock = false;

    // Start is called before the first frame update
    void Start()
    {
        
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

                SEManager.Instance.Play(SEPath.SUZU1);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                SEManager.Instance.Play(SEPath.SUZU1);

            }
        }
        if (EnterLock == false)
        {

            if (Input.GetKeyDown(KeyCode.Return))
            {
                SEManager.Instance.Play(SEPath.SUZU2);
                SEManager.Instance.Play(SEPath.HUE);

            }
        }

    }
}
