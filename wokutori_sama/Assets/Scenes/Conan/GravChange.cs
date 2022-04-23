using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravChange : MonoBehaviour
{
    float mass=5.0f;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mass = rb.mass;
        
    }

    // Update is called once per frame
    void Update()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.name == "Player2")
        {
            rb.mass=10.0f;
            Debug.Log(mass);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player2")
        {
            rb.mass=3.0f;
        }
        
    }
    /*private void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.name == "Player2")
        {
            mass=10.0f;
            Debug.Log(mass);
        }
    }
    */
}
