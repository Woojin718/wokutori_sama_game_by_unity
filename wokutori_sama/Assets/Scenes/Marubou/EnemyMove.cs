using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    private string playerTag = "Player";
    public float speed;
    public float gravity;
    public bool nonVisibleAct;

    private Rigidbody2D rb = null;
    private SpriteRenderer sr = null;
    private bool rightTleftF = false;
    public EnemyCollision checkCollision;
    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //if (sr.isVisible)
        //{
           　 //Debug.Log("画面に見えている");
        //}
    }

    void FixedUpdate()
    {
        if ((playerTransform.position - transform.position).magnitude < 10f)
        {
            if (checkCollision.isOn)
            {
                rightTleftF = !rightTleftF;
            }
            int xVector = -1;
            if (rightTleftF)
            {
                xVector = 1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            rb.velocity = new Vector2(xVector * speed, -gravity);
        }
        else
        {
            rb.Sleep();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == playerTag)
        {
            
        }
    }
}
