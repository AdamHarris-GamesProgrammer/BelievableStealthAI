using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running, Failure, Success
    }

    [HideInInspector] public State _state = State.Running;
    [HideInInspector] public bool _started = false;
    [HideInInspector] public string _guid;
    [HideInInspector] public Blackboard _blackboard;
    [TextArea] public string _description;
    public AIAgent _agent;

    //TODO: Give Access to AI Controller here

    public Vector2 _position;

    public void Abort()
    {
        BehaviorTree.Traverse(this, (node) =>
        {
            node._started = false;
            node._state = State.Running;
            node.OnStop();
        });
    }
    public State Update()
    {
        if(!_started)
        {
            OnStart();
            _started = true;
        }

        _state = OnUpdate();

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
