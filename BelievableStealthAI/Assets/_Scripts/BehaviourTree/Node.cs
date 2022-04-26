using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    //States that a node can be in
    public enum State
    {
        Running, Failure, Success
    }

    [HideInInspector] public State _state = State.Running;
    [HideInInspector] public bool _started = false;
    [HideInInspector] public string _guid;
    [HideInInspector] public Blackboard _blackboard;
    [TextArea] public string description;
    public AIAgent _agent;

    public Vector2 _position;

    //Stops the current node
    public void Abort()
    {
        BehaviorTree.Traverse(this, (node) =>
        {
            node._started = false;
            node._state = State.Running;
            node.OnStop();
        });
    }

    //Handles state update logic
    public State Update()
    {
        //If not started then call onstart
        if(!_started)
        {
            OnStart();
            _started = true;
        }

        //Call update logic
        _state = OnUpdate();
        

        //if state failed or succeeded then call stop logic
        if (_state == State.Failure || _state == State.Success)
        {
            OnStop();
            _started = false;
        }

        return _state;
    }

    public virtual Node Clone() => Instantiate(this);

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();
}
