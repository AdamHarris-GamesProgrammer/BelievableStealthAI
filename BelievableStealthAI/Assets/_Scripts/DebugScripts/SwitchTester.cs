using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTester : MonoBehaviour
{
    [SerializeField] Lightswitch _switch;
    [SerializeField] KeyCode _key;

    void Update()
    {
        //Flicks a light on or off when k is pressed
        if(Input.GetKeyDown(_key))
        {
            _switch.InteractWithObject();
        }        
    }
}
