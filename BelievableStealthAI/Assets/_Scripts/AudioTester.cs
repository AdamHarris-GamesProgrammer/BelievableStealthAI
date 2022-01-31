using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Producing Sound");
            GetComponent<AudioProducer>().ProduceSound(30.0f, 20.0f);
        }        
    }
}
