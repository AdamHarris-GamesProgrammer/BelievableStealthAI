using UnityEngine;
using UnityEngine.AI;

//Wrapper class for working with the nav mesh agent
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
        if (!_agent.isOnOffMeshLink)
        {
            //Sets the movement speed variable for the animator
            _animator.SetFloat("movementSpeed", _agent.velocity.magnitude);
        }
    }

    //Sets the agent ability to rotate
    public void Rotation(bool val)
    {
        _agent.updateRotation = val;
    }

    //Get the reamining distance between the agent and the target
    public float GetRemainingDistance()
    {
        return Vector3.SqrMagnitude(transform.position - _agent.destination);
    }

    //Sets the max speed an agent can go
    public void SetMaxSpeed(float speed)
    {
        _agent.speed = speed * _multiplier;
    }

    //Sets the agents destination to a specific position
    public void SetDestination(Vector3 position)
    {
        _agent.SetDestination(position);
    }

    
    //Sets the agents ability to move
    public void CanMove(bool val)
    {
        _agent.updatePosition = val;
    }
}
