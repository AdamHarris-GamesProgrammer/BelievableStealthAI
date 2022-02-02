using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Door : ObservableObject
{
    PlayerController _player;
    Animator _animator;



    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _animator = GetComponentInChildren<Animator>();

        //TODO: Figure out a way to have a door automatically detect if it is open or closed. 
        //CLOSED
        _currentState = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _animator.SetTrigger("openDoor");
            
        }
        else if (other.CompareTag("Player"))
        {
            _player.NearbyDoor = this;

            //TODO: How does this even work, it returns a transform either way?

            if (GetClosestSide(other.transform.position))
            {
                _startObservePosition = _sideA.position;
                _endObservePositon = _sideB.position;
            }
            else
            {
                _startObservePosition = _sideB.position;
                _endObservePositon = _sideA.position;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _animator.SetTrigger("closeDoor");

            AIAgent enemy = other.GetComponent<AIAgent>();

            Transform closest = GetClosestSide(enemy.transform.position);
            if(closest == _sideA)
            {
                enemy.CurrentRoom = _sideARoom;
            }
            else
            {
                enemy.CurrentRoom = _sideBRoom;
            }

        }
        else if (other.CompareTag("Player"))
        {
            _player.NearbyDoor = null;
        }
    }

    public void DecideAnimation()
    {
        //opened
        if (_currentState)
        {
            _animator.SetTrigger("closeDoor");
        }
        //closed
        else
        {
            _animator.SetTrigger("openDoor");
        }
    }
}
