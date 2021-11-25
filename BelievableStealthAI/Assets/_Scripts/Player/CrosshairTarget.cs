using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTarget : MonoBehaviour
{

    Camera _mainCamera;

    [SerializeField] LayerMask _mask;


    Ray ray;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        ray.origin = _mainCamera.transform.position;
        ray.direction = _mainCamera.transform.forward;

        if(Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hitInfo, 1000.0f, _mask, QueryTriggerInteraction.Ignore))
        {
            //Moves the position of the target to what we hit
            transform.position = hitInfo.point;
        }
    }
}
