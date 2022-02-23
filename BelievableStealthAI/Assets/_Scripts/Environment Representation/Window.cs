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

        if (_shouldBeOpen)
        {
            DecideAnimation();
            _currentState = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<AIAgent>().NearbyObservable = this;
        }
        else if (other.CompareTag("Player"))
        {
            _controller.NearbyWindow = this;
        }
    }

    public override void Open()
    {
        //if (_currentState) return;

        _animator.Play("WindowOpen", 0, 0.0f);
    }

    public override void Close()
    {
        //if (!_currentState) return;

        _animator.Play("WindowClose", 0, 0.0f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AIAgent enemy = other.GetComponent<AIAgent>();

            enemy.NearbyObservable = null;


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
        if (_currentState)
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
