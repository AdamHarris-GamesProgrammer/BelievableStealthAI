using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObservableObject : MonoBehaviour
{
    [SerializeField] protected bool _originalState;
    protected bool _currentState;
    [SerializeField] protected bool _changedState;
    [SerializeField] protected bool _changedStateRecently;
    [SerializeField] float _eventDuration = 180.0f;

    [SerializeField] ObservableType _type;

    [SerializeField] Transform _sideA;
    [SerializeField] Transform _sideB;

    public ObservableType Type { get => _type; }

    protected Vector3 _startObservePosition;
    protected Vector3 _endObservePositon;

    public Vector3 StartObservePosition { get => _startObservePosition; }
    public Vector3 EndObservePosition { get => _endObservePositon; }

    float _timer = 0.0f;

    public bool HasRecentlyChanged { get => _changedStateRecently; }
    public bool CurrentState { get => _currentState; }

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

    public Transform GetClosestSide(Vector3 pos)
    {
        if (_sideA == _sideB) return _sideA;

        if(Vector3.Distance(_sideA.position, pos) < Vector3.Distance(_sideB.position, pos))
        {
            return _sideA;
        }
        return _sideB;
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
