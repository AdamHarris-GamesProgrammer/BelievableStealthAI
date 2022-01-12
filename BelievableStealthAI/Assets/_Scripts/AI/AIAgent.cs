using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    BehaviorTree _behaviorTree;
    Blackboard _blackboard;
    AILocomotion _locomotion;

    PlayerController _player;

    [SerializeField] PatrolRoute _patrolRoute;
    int _currentPatrolIndex = 0;

    [SerializeField] float _walkSpeed = 1.5f;
    [SerializeField] float _patrolSpeed = 1.2f;
    [SerializeField] float _chaseSpeed = 5.4f;


    bool _haveBeenAlerted = false;
    bool _hasSeenPlayer = false;
    bool _hasHeardSound = false;
    bool _hasSeenBody = false;
    bool _currentlyAlert = false;
    bool _currentlyHearingSound = false;
    bool _currentlySeeingPlayer = false;
    bool _hasAnObjectchanged = false;

    AIAgent _agentToCheckOn = null;

    Vector3 _lastKnownPlayerPosition;

    public bool HaveBeenAlerted { get => _haveBeenAlerted;}
    public bool HasSeenPlayer { get => _hasSeenPlayer; }
    public bool HasHeardSound { get => _hasHeardSound; }
    public bool HasSeenBody { get => _hasSeenBody; }
    public bool CurrentlyAlert { get => _currentlyAlert;}
    public bool CurrentlyHearingSound { get => _currentlyHearingSound;}
    public bool CurrentlySeeingPlayer { get => _currentlySeeingPlayer;}
    public bool HasAnObjectchanged { get => _hasAnObjectchanged; }

    public Vector3 LastKnownPlayerPosition { get => _lastKnownPlayerPosition; set => _lastKnownPlayerPosition = value; }

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void Start()
    {
        _behaviorTree = GetComponent<BehaviorTreeRunner>().tree;
        _blackboard = _behaviorTree._blackboard;

        _locomotion = GetComponent<AILocomotion>();

        _blackboard._agent = this;
        _blackboard._locomotion = _locomotion;

        _blackboard.spawnPosition = transform.position;
        _blackboard.spawnOrientation = transform.forward;

        if(_patrolRoute)
        {
            _blackboard._hasPatrolRoute = true;
        }

        _blackboard._walkSpeed = _walkSpeed;
        _blackboard._patrolSpeed = _patrolSpeed;
        _blackboard._chaseSpeed = _chaseSpeed;
    }

    //TODO: Get closest patrol point method, so that ai can resume there patrol from the closest point 

    public void GetNextPatrolPoint()
    {
        _blackboard.moveToPosition = _patrolRoute.GetNextIndex(ref _currentPatrolIndex);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BodyDetected()
    {
        _hasSeenBody = true;
        if(_currentlyAlert)
        {
            return;
        }

        if(!_haveBeenAlerted)
        {
            //TODO: Play Panicked Animation and Dialogue 
        }
        else
        {

        }
    }

    public void PlayerSeen()
    {
        Debug.Log("Player Seen");
        _currentlySeeingPlayer = true;

        if(!_hasSeenPlayer)
        {
            //TODO: Play Dialogue
        }
        else
        {
            //TODO: Play Dialogue
        }
        
        if(!_currentlyAlert)
        {
            //TODO: Alert nearby allys
            _currentlyAlert = true;

        }

        _hasSeenPlayer = true;
    }

    public void LostSightOfPlayer()
    {
        _currentlySeeingPlayer = false;
        
        //TODO: Currently alert should be set to false at another point
        _currentlyAlert = false;
    }

    public void SoundHeard()
    {
        _currentlyHearingSound = true;
        _hasHeardSound = true;

        if(_currentlyAlert)
        {
            return;
        }
        else
        {
            //TODO: Play Dialogue
        }
    }

    public void CheckOn(AIAgent agent)
    {
        _agentToCheckOn = agent;
        //TODO: Play Dialogue
    }
}
