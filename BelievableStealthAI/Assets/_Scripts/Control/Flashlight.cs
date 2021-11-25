using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    Light _flashlight;

    void Awake() {
        _flashlight = GetComponent<Light>();
        _flashlight.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        //Enable/Disable the flashlight when T is pressed
        if(Input.GetKeyDown(KeyCode.T)) _flashlight.enabled = !_flashlight.enabled;

        //Rotates the flashlight based on where you are looking
        _flashlight.transform.forward = Camera.main.transform.forward;
    }
}
