using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    Animator _animator;
    NavMeshAgent _agent;

    public float Multiplier { get => _multiplier; set => _multiplier = value; }
    float _multiplier = 1.0f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _animator.SetFloat("movementSpeed", _agent.velocity.magnitude);
    }

    public void Rotation(bool v)
    {
        _agent.updateRotation = v;
    }

    public float GetRemainingDistance()
    {
        return Vector3.SqrMagnitude(transform.position - _agent.destination);
    }

    public void SetMaxSpeed(float speed)
    {
        _agent.speed = speed * _multiplier;
    }

    public void SetDestination(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    internal void CanMove(bool v)
    {
        _agent.updatePosition = v;
    }
}
