using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AILocomotion : MonoBehaviour
{
    Animator _animator;
    NavMeshAgent _agent;

    bool _canUseDoor = false;
    public bool CanUseDoor { get => _canUseDoor; set => _canUseDoor = value; }

    Door _currentDoor;
    public Door CurrentDoor { get => _currentDoor; set => _currentDoor = value; }

    Transform _thisSide;
    Transform _otherSide;

    public void SetDoorSides(Transform current, Transform other)
    {
        _thisSide = current;
        _otherSide = other;
    }

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

    IEnumerator MoveThroughDoor()
    {
        //TODO: Move all this stuff into behavior tree system
        _agent.updateRotation = false;
        transform.position = _thisSide.position;
        transform.forward = _otherSide.up;
        _animator.SetTrigger("openDoor");
        _currentDoor.GetComponentInChildren<Animator>().SetTrigger("openDoor");

        _canUseDoor = false;

        yield return null;
    }

    void Update()
    {
        _animator.SetFloat("movementSpeed", _agent.velocity.magnitude);

        if (_agent.isOnOffMeshLink)
        {
            if(_canUseDoor)
            {
                StartCoroutine(MoveThroughDoor());
            }
        }
    }

    public void SetDestination(Vector3 position)
    {
        //TODO: Adjust the Agent speed based on the context of the game  (E.g. In combat, Patroling, idling, etc).

        _agent.SetDestination(position);
    }

    //Called by animation event    
    public void FinishDoor()
    {
        Debug.Log("Finish Door");
        _agent.CompleteOffMeshLink();
        _currentDoor.GetComponentInChildren<Animator>().SetTrigger("closeDoor");
        transform.position = _otherSide.position;
        _agent.updateRotation = true;
        _canUseDoor = false;
    }
}
