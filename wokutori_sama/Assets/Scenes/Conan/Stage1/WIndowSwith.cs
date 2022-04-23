using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class WIndowSwith : MonoBehaviour
{
    GameObject icon;
    float icon_a;
    bool window_on =false;
    public GameObject messageWindow;
    GameObject player;
    FadeController script;
    public GameObject mark;
    public float d;
    

    // Start is called before the first frame update
    void Start()
    {
        icon = GameObject.Find("icon");
        player = GameObject.Find("Player2");
        
    }

    // Update is called once per frame
    void Update()
    {
        script= icon.GetComponent<FadeController>();
        icon_a = icon.GetComponent<SpriteRenderer>().color.a;
        
        Vector2 player_pos=player.transform.position;
        Vector2 mark_pos=mark.transform.position;
        Vector2 dir = player_pos - mark_pos;
        d= dir.magnitude;
        
        //Debug.Log(d);
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            if (icon_a >= 0.5)
            {
                SEManager.Instance.Play(SEPath.PAPER);

                window_on = !window_on;
            }
            if (icon.activeSelf == false)
            {
                SEManager.Instance.Play(SEPath.PAPER);

                messageWindow.SetActive(false);
                icon.SetActive(true);
            }
            
        }
        
        
        
        
        if (window_on == true)
        {
            messageWindow.SetActive(true);
            icon.SetActive(false);
            

            if (d >= 3.0f)
            {
                window_on = !window_on;
            }
        }

        if (window_on == false)
        {
            messageWindow.SetActive(false);
            icon.SetActive(true);
        }
        
    }
}
