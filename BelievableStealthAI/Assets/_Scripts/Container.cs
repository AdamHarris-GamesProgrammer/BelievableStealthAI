using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Container : MonoBehaviour
{
    bool _playerInside = false;

    public bool PlayerInside { get => _playerInside; }

    PlayerController _player;

    void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetIn()
    {
        _playerInside = true;
    }

    public void GetOut()
    {
        _playerInside = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            _player.SetContainer(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _player.SetContainer(null);
    }
}
