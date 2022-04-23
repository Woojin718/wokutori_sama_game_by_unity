using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bucket : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    public bool isPicked;
    // Start is called before the first frame update
    void Start()
    {
        isPicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player"&&isPicked==false)
        {
            //Debug.Log("!");
            player = collision.gameObject;
            //offset = transform.position - player.transform.position;
            transform.position = player.transform.position;
            isPicked = true;
        }
    }
    private void LateUpdate()
    {
        if (isPicked)
        {
            
            transform.position = Vector3.Lerp(transform.position, player.transform.position-new Vector3(0.2f,-0.2f,0), 6.0f * Time.deltaTime);
        }
    }
}
