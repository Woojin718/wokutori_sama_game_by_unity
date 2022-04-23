using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;


public class WallSeManager : MonoBehaviour
{


    public bool isWallMove = false;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WallSeTrigger")
        {
            SEManager.Instance.Stop(SEPath.GATE_OPEN);
            Debug.Log("!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WallSeTrigger")
        {
            SEManager.Instance.Play(SEPath.GATE_OPEN);
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "WallSeTrigger")
    //    {
    //        SEManager.Instance.Stop(SEPath.GATE_OPEN);
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "WallSeTrigger")
    //    {
    //        SEManager.Instance.Play(SEPath.GATE_OPEN);
    //    }
    //}
}
