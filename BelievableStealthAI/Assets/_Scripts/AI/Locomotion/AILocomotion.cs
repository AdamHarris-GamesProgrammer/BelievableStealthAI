using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    Animator _animator;
    NavMeshAgent _agent;

    public float GetRemainingDistance()
    {
        //return _agent.remainingDistance;
        return Vector3.SqrMagnitude(transform.position - _agent.destination);
    }

    public void SetMaxSpeed(float speed)
    {
        _agent.speed = speed;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _agent.autoTraverseOffMeshLink = false;
       
    }

    void Update()
    {
        _animator.SetFloat("movementSpeed", _agent.velocity.magnitude);
    }

    public void SetDestination(Vector3 position)
    {
        _agent.SetDestination(position);
    }
}
