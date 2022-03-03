using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Producing Sound");
            AudioProducer.ProduceSound(transform.position, 30.0f, 5.0f);
        }        
    }
}
