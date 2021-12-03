using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    Animator _animator;
    NavMeshAgent _agent;

    [SerializeField] Transform _targetTransform;


    bool _canUseDoor = false;
    public bool CanUseDoor { get => _canUseDoor; set => _canUseDoor = value; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();

        _agent.autoTraverseOffMeshLink = false;
    }

    void Update()
    {
        _agent.SetDestination(_targetTransform.position);
        _animator.SetFloat("movementSpeed", _agent.velocity.magnitude);

        if(_agent.isOnOffMeshLink)
        {
            if(_canUseDoor)
            {
                _animator.SetTrigger("openDoor");
                
            }

            //check if we are at door
            //is door open
            //walk through door
            //else
            //open door
            //walk through door
            //else
            //at some other nav mesh link, like a window.
        }
    }


    //Called by animation event    
    public void FinishDoor()
    {
        Debug.Log("Finish Door");
        _agent.CompleteOffMeshLink();
        _canUseDoor = false;
    }
}
