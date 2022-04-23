using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChengeScript : MonoBehaviour
{
    public GameObject trueStages;
    bool bucketBool = false;
  

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bucketBool == true)
        {
            if (Input.GetKey(KeyCode.E))
            {
                trueStages.SetActive(false);
                Debug.Log("明かりが消えた。");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bucket")
        {
            bucketBool = true;
        }
        
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bucket")
        {
            bucketBool = false;
        }
       
    }
}
