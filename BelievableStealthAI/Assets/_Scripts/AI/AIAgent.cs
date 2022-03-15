using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TGP.Control;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] PatrolRoute _patrolRoute;
    int _currentPatrolIndex = 0;

    [Header("Movement Settings")]
    [SerializeField] float _walkSpeed = 1.5f;
    [SerializeField] float _patrolSpeed = 1.2f;
    [SerializeField] float _chaseSpeed = 5.4f;

    [Header("AI Check on Settings")]
    [SerializeField] float _checkOnThreshold = 1.0f;

    [Header("Suspicion Settings")]
    [SerializeField] float _durationForSuspiscion = 15.0f;

    [Header("Room Details")]
    [SerializeField] RoomController _currentRoom;

    float _suspiscionTimer;

    bool _haveBeenAlerted = false;
    bool _hasSeenPlayer = false;
    bool _hasHeardSound = false;
    bool _hasSeenBody = false;
    bool _currentlyAlert = false;
    bool _currentlyHearingSound = false;
    bool _currentlySeeingPlayer = false;
    bool _hasAnObjectchanged = false;
    bool _suspicious = false;
    bool _stopSearching = false;
    bool _isDistracted = false;
    bool _isInAnimation = false;

    Vector3 _pointOfSound;
    Vector3 _distractedPoint;
    Vector3 _lastKnownPlayerPosition;

    BehaviorTree _behaviorTree;
    Blackboard _blackboard;
    AILocomotion _locomotion;
    Animator _animator;
    DialogueController _dialogueController;
    PlayerController _player;
    AIAgent _agentToCheckOn = null;
    Lightswitch _lightswitch;
    ObservableObject _nearbyObservable;
    GameObject _deadAgent;

    Dictionary<AIAgent, float> _aiLastSeen = new Dictionary<AIAgent, float>();

    public bool IsDistracted { get => _isDistracted; set => _isDistracted = value; }
    public bool Suspicious { get => _suspicious; set => _suspicious = false; }
    public bool HaveBeenAlerted { get => _haveBeenAlerted; }
    public bool HasSeenPlayer { get => _hasSeenPlayer; }
    public bool HasHeardSound { get => _hasHeardSound; }
    public bool HasSeenBody { get => _hasSeenBody; set => _hasSeenBody = value; }
    public bool CurrentlyAlert { get => _currentlyAlert; set => _currentlyAlert = value; }
    public bool CurrentlyHearingSound { get => _currentlyHearingSound; }
    public bool CurrentlySeeingPlayer { get => _currentlySeeingPlayer; }
    public bool HasAnObjectchanged { get => _hasAnObjectchanged; }
    public bool StopSearching { get => _stopSearching; set => _stopSearching = value; }
    public bool IsInAnimation { get => _isInAnimation; set => _isInAnimation = value; }


    public Vector3 DistractionPoint { get => _distractedPoint; set => _distractedPoint = value; }
    public ObservableObject NearbyObservable { get => _nearbyObservable; set => _nearbyObservable = value; }
    public RoomController CurrentRoom { get => _currentRoom; set => _currentRoom = value; }
    public GameObject DeadAgent { get => _deadAgent; set => _deadAgent = null; }
    public Lightswitch ChangedLightswitch { get => _lightswitch; }
    public Vector3 LastKnownPlayerPosition { get => _lastKnownPlayerPosition; set => _lastKnownPlayerPosition = value; }
    public Vector3 PointOfSound { get => _pointOfSound; set => _pointOfSound = value; }
    public AIAgent AgentToCheckOn { get => _agentToCheckOn; set => _agentToCheckOn = value; }
    public Animator Anim { get => _animator; }


    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
        _animator = GetComponent<Animator>();
        _dialogueController = GetComponent<DialogueController>();
    }

    private void Start()
    {
        if(GetComponent<BehaviorTreeRunner>() != null && GetComponent<BehaviorTreeRunner>().tree != null)
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


        foreach(AIAgent agent in _currentRoom.AgentsInRoom)
        {
            if (agent == this) continue;
            _aiLastSeen.Add(agent, 0.0f);
        }
    }

    public void PlaySound(SoundType type)
    {
        _dialogueController.PlaySound(type);
    }
    public void RadioAllyToCheckOn()
    {
        //Debug.Log(transform.name + " is checking on: " + _agentToCheckOn.transform.name);
        _dialogueController.PlaySound(SoundType.CheckOnAlly);
        _agentToCheckOn.Respond(this);
    }

    public void Respond(AIAgent caller)
    {
        //Debug.Log(transform.name + " is being checked on by: " + caller.transform.name);
        if (GetComponent<Health>().IsDead)
        {
            caller._blackboard._response = false;
        }
        else
        {
            _dialogueController.PlaySound(SoundType.ReturnCall);
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

    public void Attack(bool autoKill = false)
    {
        //Debug.Log("Attack method");
        _animator.SetTrigger("attack");

        if (autoKill)
        {
            _player.TakeHit();
        }
    }

    public void SeenChangedObject(ObservableObject obj)
    {
        //Debug.Log(transform.name + " has seen a changed object");
        if (_currentlyAlert)
        {
            return;
        }

        if (obj.Type == ObservableType.Door)
        {
            if (obj.CurrentState) _dialogueController.PlaySound(SoundType.DoorOpen);
            else _dialogueController.PlaySound(SoundType.DoorClosed);
        }
        else if (obj.Type == ObservableType.Window)
        {
            if (obj.CurrentState) _dialogueController.PlaySound(SoundType.WindowOpen);
            else _dialogueController.PlaySound(SoundType.WindowClosed);
        }

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
            if (!_currentlySeeingPlayer)
            {
                _suspiscionTimer += Time.deltaTime;

                if (_suspiscionTimer > _durationForSuspiscion)
                {
                    _suspiscionTimer = 0.0f;
                }
            }
        }


        if (_agentToCheckOn != null) return;

        Dictionary<AIAgent, float> modifyVals = new Dictionary<AIAgent, float>();

        Vector3 raycastOrigin = transform.position + (Vector3.up * 1.5f);
        foreach (AIAgent agent in _aiLastSeen.Keys)
        {
            Vector3 raycastDestination = agent.transform.position + (Vector3.up * 1.5f);
            Vector3 direction = raycastDestination - raycastOrigin;
            if(Physics.Raycast(raycastOrigin, direction, out RaycastHit hit, 35.0f, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    //Can be currently seen
                    modifyVals.Add(agent, 0.0f);
                }
                else
                {
                    //Cannot be seen currently
                    float val = _aiLastSeen[agent];
                    val += Time.deltaTime;
                    //_aiLastSeen[agent] = val;
                    modifyVals.Add(agent, val);

                    if (val > _checkOnThreshold)
                    {
                        CheckOn(agent);

                        modifyVals[agent] = 0.0f;
                    }
                }
            }
        }

        foreach (AIAgent agent in modifyVals.Keys)
        {
            _aiLastSeen[agent] = modifyVals[agent];
        }
    }

    public void Distracted(Vector3 impactPoint)
    {
        if (_currentlyAlert) return;

        _isDistracted = true;
        _distractedPoint = impactPoint;
    }

    public void BodyDetected(GameObject agent)
    {
        Debug.Log(transform.name + " has seen a body");
        _deadAgent = agent;
        _hasSeenBody = true;
        if (_currentlyAlert)
        {
            return;
        }

        if (!_haveBeenAlerted)
        {
            //TODO: Play Panicked Animation
            _dialogueController.PlaySound(SoundType.FindingBody);
        }
        else
        {

        }
    }

    public void PlayerSeen()
    {
        //Debug.Log("Player Seen");

        if (!_hasSeenPlayer)
        {
            _dialogueController.PlaySound(SoundType.FirstTimeSeeingPlayer);
            _animator.Play("Surprised");
            //Debug.Log(transform.name + " has seen the player for the first time");
        }
        else
        {
            if (!_currentlySeeingPlayer)
            {
                _dialogueController.PlaySound(SoundType.SeeingPlayerAgain);
                //Debug.Log(transform.name + " has seen the player");
            }
        }
        _currentlySeeingPlayer = true;

        if (!_currentlyAlert)
        {
            _suspicious = true;
            _currentlyAlert = true;
        }

        _hasSeenPlayer = true;
    }

    public void LightSwitchChanged(Lightswitch ls)
    {
        //Debug.Log(transform.name + " has seen a changed lightbulb");
        if (_currentlyAlert)
        {
            return;
        }

        _lightswitch = ls;

        //Trigger the object investigation branch
        SeenChangedObject(ls);

        if (_lightswitch.CurrentState)
        {
            _dialogueController.PlaySound(SoundType.LightsOn);
        }
        else
        {
            _dialogueController.PlaySound(SoundType.LightsOff);
        }
    }

    public void LostSightOfPlayer()
    {
        _currentlySeeingPlayer = false;
    }

    public void SoundHeard()
    {
        if (_currentlyHearingSound) return;

        Debug.Log(transform.name + " has heard something");
        _hasHeardSound = true;

        if (_currentlyAlert)
        {
            return;
        }
        else
        {
            if (!_currentlyHearingSound)
            {
                _dialogueController.PlaySound(SoundType.HeardSomething);
            }
        }
        _currentlyHearingSound = true;
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
            _dialogueController.PlaySound(SoundType.NothingThere);
        }
    }

    public void ForceAlertAll()
    {
        Debug.Log("force alert all");
        _dialogueController.PlaySound(SoundType.EveryoneSearchPrompt);
        RoomController[] rooms = FindObjectsOfType<RoomController>();
        foreach (RoomController room in rooms)
        {
            room.AgentsInRoom.ForEach((AIAgent agent) => agent.ForceAlert(false));
        }
    }

    public void ForceAlert(bool playDialoge)
    {
        Debug.Log("force alerted");
        if (playDialoge) _dialogueController.PlaySound(SoundType.SearchPrompt);

        _suspicious = true;
        _currentlyAlert = true;
    }

    public void CheckOn(AIAgent agent)
    {
        Debug.Log(transform.name + " needs to check on: " + agent.name);

        //We already have an agent to check on
        if (_agentToCheckOn != null) return;

        //if we are not currently alert
        if (!_currentlyAlert)
        {
            //Set the new agent to check on
            _agentToCheckOn = agent;
        }
    }

    public void FinishedAnimation()
    {
        _isInAnimation = false;
        Debug.Log("Finished animation");
        
    }

    public void ForceStopAnimtion()
    {
        _animator.enabled = false;

        _animator.enabled = true;
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
