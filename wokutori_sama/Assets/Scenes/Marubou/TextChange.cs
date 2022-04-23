using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChange : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Cube_1")
        {
            text.text = "Rキーで調べる";
        }
        if (collision.gameObject.name == "Cube_2")
        {
            text.text = "矢印キーで移動";
        }


        if (collision.gameObject.name == "Cube0")
        {
            text.text = "Space で灯りの操作";
        }
        if (collision.gameObject.name == "Cube1")
        {
            text.text = "Shift+矢印キーで箱の移動";
        }
        if (collision.gameObject.name == "Cube2")
        {
            text.text = "灯りをつけてる時にしか見えないものがある";
        }
        if (collision.gameObject.name == "Cube3")
        {
            text.text = "篝火に触れるとセーブされる";
        }
        if (collision.gameObject.name == "Cube4")
        {
            text.text = "Rキーで松明に火をつける";
        }
        
        //else
        //{
        //    text.text = "";
        //}
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TutorialTrigger")
        {
            text.text = "";
        }

    }
}