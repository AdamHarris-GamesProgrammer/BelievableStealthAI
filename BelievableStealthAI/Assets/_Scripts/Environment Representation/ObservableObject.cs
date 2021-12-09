using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObservableObject : MonoBehaviour
{
    [SerializeField] protected bool _originalState;
    bool _currentState;
    [SerializeField] protected bool _changedState;
    [SerializeField] protected bool _changedStateRecently;
    [SerializeField] float _eventDuration = 180.0f;

    protected Vector3 _startObservePosition;
    protected Vector3 _endObservePositon;

    public Vector3 StartObservePosition { get => _startObservePosition; }
    public Vector3 EndObservePosition { get => _endObservePositon; }

    float _timer = 0.0f;

    public bool HasRecentlyChanged { get => _changedStateRecently; }

    private void Awake()
    {
        _currentState = _originalState;
    }

    void FixedUpdate()
    {
        if (_changedState)
        {
            _timer += Time.fixedDeltaTime;

            if (_timer > _eventDuration)
            {
                _timer = 0.0f;
                _changedStateRecently = false;
            }
        }
    }

    public void InteractWithObject()
    {
        _currentState = !_currentState;

        if(_currentState != _originalState)
        {
            _changedState = true;
            _changedStateRecently = true;
        }
        else
        {
            _changedState = false;
            _changedStateRecently = false;
        }
    }
}
