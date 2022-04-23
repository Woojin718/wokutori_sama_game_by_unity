using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour
{
    //public GameObject box;
    Rigidbody2D rb;
    GameObject player;
    PlayerMover script;
    bool push;
    bool nowPushing=false;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        script = player.GetComponent<PlayerMover>();
        
              

    }

    // Update is called once per frame
    void Update()
    {
        //push = script.IsPushing.Value;

        /*if (push==true && Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("PushOn");
            rb.velocity = new Vector2(3.0f, rb.velocity.y);
        }

        else if(push==true && Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("PullOn");
            rb.velocity = new Vector2(-3.0f, rb.velocity.y);
        }
        */

        if(nowPushing==true && push==true && Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("PushOn");
            rb.velocity = new Vector2(3.0f, rb.velocity.y);
        }
        else if(nowPushing == true && push == true && Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-3.0f, rb.velocity.y);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            nowPushing = true;
        }
    }


}
