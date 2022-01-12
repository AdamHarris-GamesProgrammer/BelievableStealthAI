using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Door : ObservableObject
{
    [SerializeField] Transform sideA;
    [SerializeField] Transform sideB;
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
            Debug.Log(other.name);
            _animator.SetTrigger("openDoor");
            
        }
        else if (other.CompareTag("Player"))
        {
            _player.NearbyDoor = this;


            if (GetClosestDoorSide(other.transform.position))
            {
                _startObservePosition = sideA.position;
                _endObservePositon = sideB.position;
            }
            else
            {
                _startObservePosition = sideB.position;
                _endObservePositon = sideA.position;
            }
        }
    }

    private Transform GetClosestDoorSide(Vector3 pos)
    {
        if (Vector3.Distance(pos, sideA.position) < Vector3.Distance(pos, sideB.position))
        {
            return sideA;
        }

        return sideB;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _animator.SetTrigger("closeDoor");
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
