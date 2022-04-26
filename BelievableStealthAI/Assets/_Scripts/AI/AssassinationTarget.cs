using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinationTarget : MonoBehaviour
{
    PlayerController _player;
    AIAgent _agent;

    void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _agent = GetComponentInParent<AIAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.NearbyAgent = _agent;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.NearbyAgent = null;
        }
    }
}
