using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGP.Control;

public class Window : ObservableObject
{
    Animator _animator;
    PlayerController _controller;

    [SerializeField] Collider _colliderToDisable;
    [SerializeField] bool _shouldBeOpen;
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _controller = FindObjectOfType<PlayerController>();

        if(_shouldBeOpen)
        {
            DecideAnimation();
            _currentState = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            _animator.SetTrigger("openWindow");
        }
        else if(other.CompareTag("Player"))
        {
            _controller.NearbyWindow = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _animator.SetTrigger("closeWindow");


            AIAgent enemy = other.GetComponent<AIAgent>();

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
            _controller.NearbyWindow = null;
        }
    }

    public void DecideAnimation()
    {
        if(_currentState) 
        {
            _animator.SetTrigger("closeWindow");
            _colliderToDisable.enabled = true;
        }
        else
        {
            _animator.SetTrigger("openWindow");
            _colliderToDisable.enabled = false;
        }
    }
}
