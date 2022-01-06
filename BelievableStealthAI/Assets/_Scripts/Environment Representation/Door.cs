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
        if(other.CompareTag("Enemy"))
        {
            AILocomotion ai = other.GetComponent<AILocomotion>();
            ai.CanUseDoor = true;

            ai.CurrentDoor = this;
            if (GetClosestDoorSide(other.transform.position) == sideA)
            {
                ai.SetDoorSides(sideA, sideB);
            }
            else
            {
                ai.SetDoorSides(sideB, sideA);
            }
        }
        else if(other.CompareTag("Player"))
        {
            _player.SetDoor(this);


            if(GetClosestDoorSide(other.transform.position))
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
            //Debug.Log("Close to Side A");
            return sideA;
        }

        //Debug.Log("Close to Side B");
        return sideB;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AILocomotion ai = other.GetComponent<AILocomotion>();
            ai.CanUseDoor = false;
        }
        else if(other.CompareTag("Player"))
        {
            _player.SetDoor(null);
        }
    }

    public void DecideAnimation ()
    {
        //opened
        if(_currentState)
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
