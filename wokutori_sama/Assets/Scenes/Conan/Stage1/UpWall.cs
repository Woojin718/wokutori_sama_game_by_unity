using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;


public class UpWall : MonoBehaviour
{
    Vector3 nowPos;
    public GameObject wall;
    bool upStart = false;
    public int boxCount = 0;
    

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //wall = GameObject.Find("Wall");
        
        Vector3 nowPos = wall.transform.position;
        wall.transform.position = new Vector3(nowPos.x, nowPos.y,nowPos.z);
        //float y = nowPos.y;
        
        if (nowPos.y < -5.3 && upStart ==true)
        {
            wall.transform.Translate(0, 0.01f, 0);
        }

        if (nowPos.y > -7.0 && upStart == false)
        {
            wall.transform.Translate(0,-0.01f,0);
        }
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag =="Carriable"　&& boxCount == 0)
        {
            upStart = true;
            boxCount += 1;
            SEManager.Instance.Play(SEPath.GATE_SWITCH);
        }
        if (collision.gameObject.tag == "Player" && boxCount ==0)
        {
            upStart = true;
            SEManager.Instance.Play(SEPath.GATE_SWITCH);
            Debug.Log(upStart);
        }
    }
    
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && boxCount==0)
        {
            
            upStart = false;
            SEManager.Instance.Play(SEPath.GATE_SWITCH);
        }
    }
    
}
