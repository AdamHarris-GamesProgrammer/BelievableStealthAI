using System;
using System.Collections;
using System.Collections.Generic;
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
            //DecideAnimation();
            _currentState = true;
            _animator.Play("DoorOpen", 0, 0.0f);
        }

        _originalState = _currentState;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Get the AIAgent component and set the nearby observable to this
            AIAgent enemy = other.GetComponent<AIAgent>();
            enemy.NearbyObservable = this;

            //Find the closest side to the enemy
            Transform closest = GetClosestSide(enemy.transform.position);
            enemy.CurrentRoom = closest == _sideA ? _sideARoom : _sideBRoom;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Get the AIAgent component and set the nearby observable to this
            AIAgent enemy = other.GetComponent<AIAgent>();
            enemy.NearbyObservable = this;

            //Find the closest side to the enemy
            Transform closest = GetClosestSide(enemy.transform.position);
            enemy.CurrentRoom = closest == _sideA ? _sideARoom : _sideBRoom;

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
        //If the enemy exists this trigger
        if (other.CompareTag("Enemy"))
        {
            //Get the AIAgent component and set the nearby ovservable to null
            AIAgent enemy = other.GetComponent<AIAgent>();
            enemy.NearbyObservable = null;

            Transform closest = GetClosestSide(enemy.transform.position);
            //Selects the current room for the enemy 
            enemy.CurrentRoom = closest == _sideA ? _sideARoom : _sideBRoom;

        }
        //If the player exists this trigger then set the nearby door variable to null
        else if (other.CompareTag("Player"))
        {
            _player.NearbyDoor = null;
        }
    }

    public override void Open()
    {
        if (_currentState) return;

        //Play the door open animation
        _animator.Play("DoorOpen", 0, 0.0f);
        PlaySFX();
    }

    public override void Close()
    {
        if (!_currentState) return;
            
        //Play the door close animation
        _animator.Play("DoorClose", 0, 0.0f);
        PlaySFX();
    }

    public override void DecideAnimation()
    {
        //opened
        if (_currentState) Close();
        //closed
        else Open();
    }
}
