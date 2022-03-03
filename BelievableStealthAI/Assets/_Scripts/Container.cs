using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Container : PointOfInterest
{
    PlayerController _player;
    void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            _player.NearbyContainer = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _player.NearbyContainer = null;
    }
}
