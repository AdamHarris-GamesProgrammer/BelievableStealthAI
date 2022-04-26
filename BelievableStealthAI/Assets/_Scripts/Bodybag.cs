using System.Collections;
using System.Collections.Generic;
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
        //If the player enters this trigger then set the nearby bodybag variable to this 
        if(other.CompareTag("Player"))
        {
            _player.NearbyBodybag = this;
        }        
    }
    private void OnTriggerExit(Collider other)
    {
        //If the player leaves this trigger then set the nearby bodybag variable to null
        if (other.CompareTag("Player"))
        {
            _player.NearbyBodybag = null;
        }
    }
}
