using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : ObservableObject
{
    [SerializeField] Transform sideA;
    [SerializeField] Transform sideB;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AILocomotion ai = other.GetComponent<AILocomotion>();
            ai.CanUseDoor = true;

            ai.CurrentDoor = this;
            if (GetClosestDoorSide(other.transform.position) == sideA)
            {
                ai.SetDoorSides(sideA, sideB);
            }
            else
            {
                ai.SetDoorSides(sideB, sideA);
            }
        }
        else if(other.CompareTag("Player"))
        {
            //Open door
            InteractWithObject();

            if(GetClosestDoorSide(other.transform.position))
            {
                _startObservePosition = sideA.position;
                _endObservePositon = sideB.position;
            }
            else
            {
                _startObservePosition = sideB.position;
                _endObservePositon = sideA.position;
            }
        }
    }

    private Transform GetClosestDoorSide(Vector3 pos)
    {
        if (Vector3.Distance(pos, sideA.position) < Vector3.Distance(pos, sideB.position))
        {
            Debug.Log("Close to Side A");
            return sideA;
        }

        Debug.Log("Close to Side B");
        return sideB;
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
