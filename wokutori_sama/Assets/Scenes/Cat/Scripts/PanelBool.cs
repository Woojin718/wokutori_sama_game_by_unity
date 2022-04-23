using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelBool : MonoBehaviour
{
    public bool PanelOn = false;
    public bool EnterLock = false;
    int select;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        select = this.GetComponent<KiokuPanelScript>().select;
        if (EnterLock == false)
        {
            if (select <= 7 && select > 0)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    PanelOn = !PanelOn;
                    EnterLock = true;
                    Invoke("EnterUnlock", 2.0f);
                }
            }
        }
        
        
    }
    void EnterUnlock()
    {
        Debug.Log("エンターロック解除");
        EnterLock = false;
    }
}
