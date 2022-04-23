using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class gameoverScript : MonoBehaviour
{
    public GameObject GameOverText;
    public GameObject Player;
    public GameObject CameraPosition;
    public CinemachineVirtualCamera CMvcam2;
    bool Over = false;

    // Start is called before the first frame update
    void Start()
    {
        GameOverText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Over == true)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                Debug.Log("!");
                SceneManager.LoadScene("sougen");
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GameOver")
        {
            GameOverText.SetActive(true);
            Vector3 tmp = GameObject.Find("Player").transform.position;
            GameObject.Find("Player").transform.position = new Vector3(tmp.x, tmp.y, tmp.z);
            var cameraTarget = Instantiate(CameraPosition, new Vector3(tmp.x, tmp.y, tmp.z), Quaternion.identity);
            CMvcam2.Priority = 10;
            CMvcam2.m_Follow = cameraTarget.transform;
            Over = true;

        }
        
    }
}
