using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torchScript : MonoBehaviour
{
    public GameObject fakeStage;
    public GameObject lightRange;
    bool torchBool = false;

    // Start is called before the first frame update
    void Start()
    {
        lightRange.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (torchBool == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                fakeStage.SetActive(false);
                lightRange.SetActive(true);
                Debug.Log("松明に火を灯した。");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            torchBool = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            torchBool = false;
        }
    }
}
