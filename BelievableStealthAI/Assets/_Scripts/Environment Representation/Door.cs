using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AILocomotion ai = other.GetComponent<AILocomotion>();
            ai.CanUseDoor = true;
            Debug.Log("AI Can use door");

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AILocomotion ai = other.GetComponent<AILocomotion>();
            ai.CanUseDoor = false;
            Debug.Log("AI Cannot use door");
        }
    }
}
