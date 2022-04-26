using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //If the player enters this trigger, then set the win condition
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Win();
        }
    }
}
