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

            if(Vector3.Distance(transform.position, sideA.position) < Vector3.Distance(transform.position, sideB.position))
            {
                
                ai.SetDoorSides(sideB, sideA);
            }
            else
            {
                ai.SetDoorSides(sideA, sideB);
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
