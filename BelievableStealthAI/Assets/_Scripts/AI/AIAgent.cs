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
