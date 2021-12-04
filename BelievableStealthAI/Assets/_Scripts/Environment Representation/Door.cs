using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform sideA;
    [SerializeField] Transform sideB;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AILocomotion ai = other.GetComponent<AILocomotion>();
            ai.CanUseDoor = true;


            if(Vector3.Distance(other.transform.position, sideA.position) < Vector3.Distance(other.transform.position, sideB.position))
            {
                ai.SetDoorSides(sideA, sideB);
            }
            else
            {
                ai.SetDoorSides(sideB, sideA);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AILocomotion ai = other.GetComponent<AILocomotion>();
            ai.CanUseDoor = false;
        }
    }
}
