using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTester : MonoBehaviour
{
    [SerializeField] Lightswitch _switch;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            _switch.InteractWithObject();
            _switch.HandleLogic();
        }        
    }
}
