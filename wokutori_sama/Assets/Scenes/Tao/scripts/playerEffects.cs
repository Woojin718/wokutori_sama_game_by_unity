using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;


public class playerEffects : MonoBehaviour
{
    public bool isHolding;
    static bool isFilled;
    int fillLevel;
    public GameObject[] water;
    public GameObject bucket;
    public Sprite bucketSprite_w;
    public Sprite bucketSprite_e;

    float watertimer;
    // Start is called before the first frame update
    void Start()
    {
        //transform.position = Vector3.zero;
        isHolding = false;
        isFilled = false;
        fillLevel = 0;
        watertimer = 5.0f;
        water[0].SetActive(false);
        water[1].SetActive(false);
        water[2].SetActive(false);
        bucket.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (transform.position.x < 40 && transform.position.x > 35)
            {
                if (fillLevel < 3 && isFilled == true)
                {
                    isFilled = false;
                    fillLevel++;
                    water[fillLevel - 1].SetActive(true);
                    bucket.GetComponent<SpriteRenderer>().sprite = bucketSprite_e;
                    SEManager.Instance.Play(SEPath.WATER_BUCKET);
                }
                else if (fillLevel > 0 && isFilled == false)
                {
                    isFilled = true;
                    fillLevel--;
                    water[fillLevel].SetActive(false);
                    bucket.GetComponent<SpriteRenderer>().sprite = bucketSprite_w;
                    SEManager.Instance.Play(SEPath.WATER_BUCKET);
                }
            }
        }

        if(bucket.activeSelf==false && transform.position.x > 10)
        {
            bucket.SetActive(true);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Drops")
        {
            if (isHolding == false||isFilled==true)
            {
                transform.position = Vector3.zero;
                SEManager.Instance.Play(SEPath.WATER_ENTER);
            }
            else
            {
                isFilled = true;
                bucket.GetComponent<SpriteRenderer>().sprite = bucketSprite_w;
                SEManager.Instance.Play(SEPath.WATER_IN);
            }

            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "bucket"&&isHolding==false)
        {
            isHolding = true;
            bucket = collision.gameObject;
            SEManager.Instance.Play(SEPath.BUCKET_GET);
        }
        //if (collision.gameObject.name == "WaterEnter")
        //{
        //    if (watertimer == 5.0f)
        //    {
        //    SEManager.Instance.Play(SEPath.WATER_ENTER);
        //    }
        //}

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "water")
        {
            
            watertimer -= Time.deltaTime;
            
            if (watertimer < 0)
            {
                transform.position = Vector3.zero;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "water")
        {
            watertimer = 5.0f;
            //SEManager.Instance.Play(SEPath.WATER_ENTER);
        }
    }



}
