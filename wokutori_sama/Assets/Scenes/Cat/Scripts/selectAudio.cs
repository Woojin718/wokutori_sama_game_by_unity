using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KanKikuchi.AudioManager;

public class selectAudio : MonoBehaviour
{
    //public AudioSource button_AudioSource;
    //public AudioSource enter_AudioSource;


    // Start is called before the first frame update
    void Start()
    {
        //button_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            //button_AudioSource.PlayOneShot(button_AudioSource.clip);
            //System20???SE???
            SEManager.Instance.Play(SEPath.SUZU1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SEManager.Instance.Play(SEPath.SUZU1);
            //button_AudioSource.PlayOneShot(button_AudioSource.clip);

        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SEManager.Instance.Play(SEPath.SUZU2);
            SEManager.Instance.Play(SEPath.HUE);
            //enter_AudioSource.PlayOneShot(enter_AudioSource.clip);

        }
    }
}
