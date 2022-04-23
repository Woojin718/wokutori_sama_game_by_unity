using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroText : MonoBehaviour
{
    GameObject player;
    public GameObject board;
    public Text text;
    private float fadeTime=1f;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player2");
        text.gameObject.GetComponent<CanvasGroup>().alpha=0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 player_pos=player.transform.position;
        Vector2 board_pos=board.transform.position;
        Vector2 dir = player_pos - board_pos;
        float d = dir.magnitude;
        
        if ((d<=2.0f))
        {
            timer += Time.deltaTime;
            Debug.Log(d);
            //text.gameObject.SetActive(true);
            text.gameObject.GetComponent<CanvasGroup>().alpha = timer/fadeTime;
        }
        else
        {
            //text.gameObject.SetActive(false);
            timer=0;
            timer+=Time.deltaTime;
            text.gameObject.GetComponent<CanvasGroup>().alpha -= timer/fadeTime;
        }
    }
    
}


