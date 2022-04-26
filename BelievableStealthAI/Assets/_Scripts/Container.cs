using System.Collections;
using System.Collections.Generic;
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
        //if the player enters this trigger then set the nearby container to this 
        if(other.CompareTag("Player"))
            _player.NearbyContainer = this;
    }

    private void OnTriggerExit(Collider other)
    {
        //if the player leaves this trigger then set the nearby container to null
        if (other.CompareTag("Player"))
            _player.NearbyContainer = null;
    }
}
