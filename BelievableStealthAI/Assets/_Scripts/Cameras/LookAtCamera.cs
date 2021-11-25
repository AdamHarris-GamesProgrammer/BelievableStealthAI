using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform _mainCam;

    void Awake()
    {
        _mainCam = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Makes an object look at the camera
        transform.LookAt(_mainCam);
    }
}
