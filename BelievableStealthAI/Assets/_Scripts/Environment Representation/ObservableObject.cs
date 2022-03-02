using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;



public class ObservableObject : MonoBehaviour
{
    [SerializeField] protected bool _originalState;
    protected bool _currentState;
    [SerializeField] protected bool _changedState;
    [SerializeField] protected bool _changedStateRecently;
    [SerializeField] float _eventDuration = 180.0f;

    [SerializeField] ObservableType _type;

    [SerializeField] protected Transform _sideA;
    [SerializeField] protected Transform _sideB;

    public ObservableType Type { get => _type; }

    protected Vector3 _startObservePosition;
    protected Vector3 _endObservePositon;

    protected Animator _animator;

    public Vector3 StartObservePosition { get => _startObservePosition; }
    public Vector3 EndObservePosition { get => _endObservePositon; }

    [SerializeField] protected RoomController _sideARoom;
    [SerializeField] protected RoomController _sideBRoom;

    protected PlayerController _player;

    float _timer = 0.0f;

    public void SetSideARoom(RoomController room)
    {
        _sideARoom = room;
    }

    public void SetSideBRoom(RoomController room)
    {
        _sideBRoom = room;
    }

    public void DecideRoom(RoomController room)
    {
        Transform closestSide = GetClosestSide(room.transform.position);
        if(closestSide == _sideA)
        {
            SetSideARoom(room);
        }
        else
        {
            SetSideBRoom(room);
        }
    }

    public bool HasRecentlyChanged { get => _changedStateRecently; }
    public bool CurrentState { get => _currentState; }

    protected void Awake()
    {
        _currentState = _originalState;
        _animator = GetComponentInChildren<Animator>();
        _player = FindObjectOfType<PlayerController>();
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

    public virtual void Open() {}

    public virtual void Close() {}

    public Transform GetClosestSide(Vector3 pos)
    {
        if (_sideA == _sideB) return _sideA;

        if(Vector3.Distance(_sideA.position, pos) < Vector3.Distance(_sideB.position, pos))
        {
            return _sideA;
        }
        return _sideB;
    }

    public Transform GetOppositeSide(Transform side)
    {
        if(_sideA == side)
        {
            return _sideB;
        }
        return _sideA;
    }

    public virtual void InteractAction() { }
    public virtual void DecideAnimation() { }

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

        InteractAction();
    }
}
