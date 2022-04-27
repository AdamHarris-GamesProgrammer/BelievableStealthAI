using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableTest : MonoBehaviour
{
    [SerializeField] ObservableObject _obvObject;
    [SerializeField] KeyCode _key;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            _obvObject.DecideAnimation();
            _obvObject.InteractWithObject();
        }        
    }
}
