using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterDropGenerator : MonoBehaviour
{
    public float maxInterval = 3.0f;
    float timer;
    float randomInterval;
    float randomX;
    public float maxX = 20;
    public float minX = 1;

    public GameObject waterDrop;

    // Start is called before the first frame update
    void Start()
    {
        timer = maxInterval;
        randomInterval = Random.Range(0, maxInterval);
        randomX = Random.Range(minX,maxX);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= randomInterval)
        {
            timer = maxInterval;
            randomInterval= Random.Range(0, maxInterval);
            Instantiate(waterDrop, new Vector3(randomX, 4.5f, 0), Quaternion.identity);
            randomX = Random.Range(minX, maxX);
        }
    }
}
