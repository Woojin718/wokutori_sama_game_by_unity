using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    GameObject player;
    float red,green,blue,alpha;
    private float fadeTime= 1.0f;
    private float timer;
    public float d ;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player2");
        //this.gameObject.GetComponent<Image>().color;
        red = this.gameObject.GetComponent<SpriteRenderer>().color.r;
        green = this.gameObject.GetComponent<SpriteRenderer>().color.g;
        blue = this.gameObject.GetComponent<SpriteRenderer>().color.b;
        alpha =this.gameObject.GetComponent<SpriteRenderer>().color.a;
        alpha=0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 player_pos=player.transform.position;
        Vector2 icon_pos=this.transform.position;
        Vector2 dir = player_pos - icon_pos;
        d = dir.magnitude;
        
        
        if (d<=3.0f)
        {
            
            timer+=Time.deltaTime;
            GetComponent<SpriteRenderer>().color= new Color(red,blue,green,alpha);
            alpha = timer/fadeTime;;
        }
        if(d >3.0f)
        {
            timer=0f;
            timer+=Time.deltaTime;
            GetComponent<SpriteRenderer>().color= new Color(red,blue,green,alpha);
            alpha -= timer/fadeTime;;
        }
    }
    
}
