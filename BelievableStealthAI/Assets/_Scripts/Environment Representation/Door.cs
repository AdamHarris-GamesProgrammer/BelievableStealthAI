using System;
using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Door : ObservableObject
{
    [SerializeField] bool _shouldBeOpen;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _player = FindObjectOfType<PlayerController>();

        //CLOSED
        _currentState = false;

        if(_shouldBeOpen)
        {
            DecideAnimation();
            _currentState = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AIAgent enemy = other.GetComponent<AIAgent>();
            enemy.NearbyObservable = this;
            Transform closest = GetClosestSide(enemy.transform.position);
            if (closest == _sideA)
            {
                enemy.CurrentRoom = _sideARoom;
            }
            else
            {
                enemy.CurrentRoom = _sideBRoom;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AIAgent enemy = other.GetComponent<AIAgent>();
            enemy.NearbyObservable = this;
            Transform closest = GetClosestSide(enemy.transform.position);
            if (closest == _sideA)
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
            _player.NearbyDoor = this;

            if (GetClosestSide(other.transform.position) == _sideA)
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
            AIAgent enemy = other.GetComponent<AIAgent>();
            enemy.NearbyObservable = null;


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

    public override void Open()
    {
        //if (_currentState) return;

        _animator.Play("DoorOpen", 0, 0.0f);
    }

    public override void Close()
    {
        //if (!_currentState) return;
            
        _animator.Play("DoorClose", 0, 0.0f);
    }

    public override void DecideAnimation()
    {
        //opened
        //Debug.Log(transform.name + " is opening or closing");
        if (_currentState)
        {
            _animator.Play("DoorClose", 0, 0.0f);
        }
        //closed
        else
        {
            _animator.Play("DoorOpen", 0, 0.0f);
        }
    }
}
