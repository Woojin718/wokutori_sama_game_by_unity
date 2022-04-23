using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearHokora : MonoBehaviour
{
    public GameObject hokora;
    bool tourchBool1 = false;
    bool tourchBool2 = false;
    bool tourchBool3 = false;

    // Start is called before the first frame update
    void Start()
    {
        hokora.gameObject.SetActive(false);  // 最初は祠消す
    }

    // Update is called once per frame
    void Update()
    {
        if(tourchBool1 == true && tourchBool2 == true && tourchBool3 == true) //松明全部つけたら祠出現
        {
            hokora.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "fire_point1")
        {
            tourchBool1 = true;
            Debug.Log("tourch1ついた");
        }

        if(collision.gameObject.name == "fire_point2")
        {
            tourchBool2 = true;
            Debug.Log("tourch2ついた");
        }

        if(collision.gameObject.name == "fire_point3")
        {
            tourchBool3 = true;
            Debug.Log("tourch3ついた");
        }
    }
}
