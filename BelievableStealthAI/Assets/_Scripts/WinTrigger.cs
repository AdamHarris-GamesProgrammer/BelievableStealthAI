using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Win();
        }
    }
}
