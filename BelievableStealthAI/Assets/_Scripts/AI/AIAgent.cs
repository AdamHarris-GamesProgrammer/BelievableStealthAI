using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TGP.Control;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    BehaviorTree _behaviorTree;
    Blackboard _blackboard;
    AILocomotion _locomotion;
    Animator _animator;

    PlayerController _player;

    [Header("Patrol Settings")]
    [SerializeField] PatrolRoute _patrolRoute;
    int _currentPatrolIndex = 0;

    [Header("Movement Settings")]
    [SerializeField] float _walkSpeed = 1.5f;
    [SerializeField] float _patrolSpeed = 1.2f;
    [SerializeField] float _chaseSpeed = 5.4f;

    [Header("Suspicion Settings")]
    [SerializeField] float _durationForSuspiscion = 15.0f;
    float _suspiscionTimer;


    bool _haveBeenAlerted = false;
    bool _hasSeenPlayer = false;
    bool _hasHeardSound = false;
    bool _hasSeenBody = false;
    bool _currentlyAlert = false;
    bool _currentlyHearingSound = false;
    bool _currentlySeeingPlayer = false;
    bool _hasAnObjectchanged = false;

    Vector3 _pointOfSound;

    AIAgent _agentToCheckOn = null;

    Vector3 _lastKnownPlayerPosition;

    Lightswitch _lightswitch;

    GameObject _deadAgent;

    [SerializeField] RoomController _currentRoom;

    public RoomController CurrentRoom { get => _currentRoom; set => _currentRoom = value; }

    public GameObject DeadAgent { get => _deadAgent; set => _deadAgent = null; }

    public bool HaveBeenAlerted { get => _haveBeenAlerted; }
    public bool HasSeenPlayer { get => _hasSeenPlayer; }
    public bool HasHeardSound { get => _hasHeardSound; }
    public bool HasSeenBody { get => _hasSeenBody; set => _hasSeenBody = value; }
    public bool CurrentlyAlert { get => _currentlyAlert; }
    public bool CurrentlyHearingSound { get => _currentlyHearingSound; }
    public bool CurrentlySeeingPlayer { get => _currentlySeeingPlayer; }
    public bool HasAnObjectchanged { get => _hasAnObjectchanged; }

    public Lightswitch ChangedLightswitch { get => _lightswitch; }

    public Vector3 LastKnownPlayerPosition { get => _lastKnownPlayerPosition; set => _lastKnownPlayerPosition = value; }
    public Vector3 PointOfSound { get => _pointOfSound; set => _pointOfSound = value; }

    public AIAgent AgentToCheckOn { get => _agentToCheckOn; set => _agentToCheckOn = value; }
    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _animator = GetComponent<Animator>();
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
        _blackboard._player = FindObjectOfType<PlayerController>();

        if (_patrolRoute != null)
        {
            Debug.Log(gameObject.name);
            _blackboard._hasPatrolRoute = true;
        }
        else
        {
            _blackboard._hasPatrolRoute = false;
        }

        _blackboard._walkSpeed = _walkSpeed;
        _blackboard._patrolSpeed = _patrolSpeed;
        _blackboard._chaseSpeed = _chaseSpeed;

        _blackboard._health = GetComponent<Health>();
    }

    public void RadioAllyToCheckOn()
    {
        Debug.Log(transform.name + " is checking on: " + _agentToCheckOn.transform.name);
        _agentToCheckOn.Respond(this);
    }

    public void Respond(AIAgent caller)
    {
        Debug.Log(transform.name + " is being checked on by: " + caller.transform.name);
        if (GetComponent<Health>().IsDead)
        {
            caller._blackboard._response = false;
        }
        else
        {
            //TODO: Play dialogue
            caller._blackboard._response = true;
        }
    }

    public bool TryAttack()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < 0.5f)
        {
            return true;
        }

        return false;
    }

    public void Attack()
    {
        //Debug.Log("Attack method");
        _animator.SetTrigger("attack");
    }

    public void SeenChangedObject(ObservableObject obj)
    {
        _blackboard._changedObservedObject = obj;
        _hasAnObjectchanged = true;
    }


    //TODO: Get closest patrol point method, so that ai can resume there patrol from the closest point 
    public void GetNextPatrolPoint()
    {
        _blackboard.moveToPosition = _patrolRoute.GetNextIndex(ref _currentPatrolIndex);
    }

    public void InvestigatedObject()
    {
        _hasAnObjectchanged = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentlyAlert)
        {
            if (!_currentlySeeingPlayer && !_currentlyHearingSound)
            {
                _suspiscionTimer += Time.deltaTime;

                if (_suspiscionTimer > _durationForSuspiscion)
                {
                    _suspiscionTimer = 0.0f;
                    _currentlyAlert = false;
                }
            }
        }
    }

    public void BodyDetected(GameObject agent)
    {
        _deadAgent = agent;
        _hasSeenBody = true;
        if (_currentlyAlert)
        {
            return;
        }

        if (!_haveBeenAlerted)
        {
            //TODO: Play Panicked Animation and Dialogue 
        }
        else
        {

        }
    }

    public void PlayerSeen()
    {
        //Debug.Log("Player Seen");
        _currentlySeeingPlayer = true;

        if (!_hasSeenPlayer)
        {
            //TODO: Play Dialogue "Whose there?"
        }
        else
        {
            //TODO: Play Dialogue "You again."
        }

        if (!_currentlyAlert)
        {
            //TODO: Alert nearby allys
            _currentlyAlert = true;

        }

        _hasSeenPlayer = true;
    }

    public void LightSwitchChanged(Lightswitch ls)
    {
        _lightswitch = ls;

        //Trigger the object investigation branch
        SeenChangedObject(ls);

        if (!_currentlyAlert)
        {
            //TODO: Play dialogue: "That's odd"
        }
        else
        {
            //TODO: Play dialogue: "I'll find you dammit
        }
    }

    public void LostSightOfPlayer()
    {
        _currentlySeeingPlayer = false;
    }

    public void SoundHeard()
    {
        _currentlyHearingSound = true;
        _hasHeardSound = true;

        if (_currentlyAlert)
        {
            return;
        }
        else
        {
            //TODO: Play Dialogue "What was that!"
        }
    }

    public void NoLongerHearingSound()
    {
        _currentlyHearingSound = false;

        if (_currentlyAlert)
        {
            return;
        }
        else
        {
            //TODO: Play dialogue "Hmm. Must've been nothing"
        }
    }

    public void ForceAlertAll()
    {
        RoomController[] rooms = FindObjectsOfType<RoomController>();
        foreach (RoomController room in rooms) 
        {
            room.AgentsInRoom.ForEach((AIAgent agent) => agent.ForceAlert());
        }
    }

    public void ForceAlert()
    {
        _currentlyAlert = true;
    }

    public void CheckOn(AIAgent agent)
    {
        //We already have an agent to check on
        if (_agentToCheckOn != null) return;

        //if we are not currently alert
        if(!_currentlyAlert)
        {
            //Set the new agent to check on
            _agentToCheckOn = agent;
        }
        //TODO: Play Dialogue "Hey Matt, are you there?"
    }

    //TODO: Replace distance with a variable
    public List<PointOfInterest> GetNearbyPointsOfInterest()
    {
        List<PointOfInterest> poi = FindObjectsOfType<PointOfInterest>().ToList();
        List<PointOfInterest> poiToRemove = new List<PointOfInterest>();

        foreach (PointOfInterest p in poi)
        {
            if (Vector3.Distance(p.transform.position, transform.position) > 15.0f)
            {
                poiToRemove.Add(p);
            }
        }

        foreach (PointOfInterest p in poiToRemove)
        {
            poi.Remove(p);
        }

        Debug.Log("Found " + poi.Count + " Points of Interest");

        return poi;
    }

    //Called by Unity Animator
    public void Bite()
    {
        Vector3 playerDir = (_player.transform.position - transform.position).normalized;
        if (Physics.Raycast(transform.position + (Vector3.up * 1.5f), playerDir, out RaycastHit hit, 1.0f, ~0, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.GetComponentInParent<PlayerController>())
            {
                _player.TakeHit();
            }
        }
    }
}
