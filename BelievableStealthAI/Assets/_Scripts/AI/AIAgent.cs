using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    BehaviorTree _behaviorTree;
    Blackboard _blackboard;
    AILocomotion _locomotion;

    [SerializeField] Transform _target;


    bool _haveBeenAlerted = false;
    bool _hasSeenPlayer = false;
    bool _hasHeardSound = false;
    bool _hasSeenBody = false;
    bool _currentlyAlert = false;
    bool _currentlyHearingSound = false;
    bool _currentlySeeingPlayer = false;
    bool _hasAnObjectchanged = false;

    AIAgent _agentToCheckOn = null;

    public bool HaveBeenAlerted { get => _haveBeenAlerted;}
    public bool HasSeenPlayer { get => _hasSeenPlayer; }
    public bool HasHeardSound { get => _hasHeardSound; }
    public bool HasSeenBody { get => _hasSeenBody; }
    public bool CurrentlyAlert { get => _currentlyAlert;}
    public bool CurrentlyHearingSound { get => _currentlyHearingSound;}
    public bool CurrentlySeeingPlayer { get => _currentlySeeingPlayer;}
    public bool HasAnObjectchanged { get => _hasAnObjectchanged; set => _hasAnObjectchanged = value; }

    private void Start()
    {
        _behaviorTree = GetComponent<BehaviorTreeRunner>().tree;
        _blackboard = _behaviorTree._blackboard;

        _locomotion = GetComponent<AILocomotion>();

        _blackboard._agent = this;
        _blackboard._locomotion = _locomotion;
        _blackboard.moveToPosition = _target.position;
    }


    // Update is called once per frame
    void Update()
    {
        _blackboard.moveToPosition = _target.position;
    }
}
