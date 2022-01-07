using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Bodybag : MonoBehaviour
{
    PlayerController _player;

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _player.NearbyBodybag = this;
        }        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.NearbyBodybag = null;
        }
    }
}
