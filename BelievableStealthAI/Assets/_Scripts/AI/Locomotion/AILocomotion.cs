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
        return _agent.remainingDistance;
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
        //TODO: Adjust the Agent speed based on the context of the game  (E.g. In combat, Patroling, idling, etc).

        _agent.SetDestination(position);
    }
}
