using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textCollider : MonoBehaviour
{
    public GameObject AlertText;

    // Start is called before the first frame update
    void Start()
    {
        AlertText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AlertText.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AlertText.SetActive(false);
        }
    }
}
