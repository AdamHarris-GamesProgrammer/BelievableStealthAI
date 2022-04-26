using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTester : MonoBehaviour
{

    [Min(0.0f)][SerializeField] float _volume = 5.0f;
    [Min(0.0f)][SerializeField] float _range = 30.0f;

    // Update is called once per frame
    void Update()
    {
        //When H is pressed product a sound from this location 
        if(Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Producing Sound");
            AudioProducer.ProduceSound(transform.position, _volume, _range);
        }        
    }
}
