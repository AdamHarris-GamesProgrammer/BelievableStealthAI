using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObservableObject : MonoBehaviour
{
    [Header("Debugging Details")]
    [SerializeField] protected bool _originalState;
    [SerializeField] protected bool _changedState;
    [SerializeField] protected bool _changedStateRecently;

    [Header("General")]
    [SerializeField] float _eventDuration = 180.0f;
    [SerializeField] ObservableType _type;
    [SerializeField] protected Transform _sideA;
    [SerializeField] protected Transform _sideB;
    [SerializeField] protected RoomController _sideBRoom;
    [SerializeField] protected RoomController _sideARoom;

    protected bool _currentState;
    public ObservableType Type { get => _type; }

    protected Vector3 _startObservePosition;
    protected Vector3 _endObservePositon;

    protected AudioSource _audioSource;

    protected Animator _animator;

    public Vector3 StartObservePosition { get => _startObservePosition; }
    public Vector3 EndObservePosition { get => _endObservePositon; }

    public bool HasRecentlyChanged { get => _changedStateRecently; }
    public bool CurrentState { get => _currentState; }

    protected PlayerController _player;

    float _timer = 0.0f;

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

    //Plays the sound effect for this object
    protected void PlaySFX()
    {
        if (_audioSource == null) _audioSource = GetComponent<AudioSource>();

        _audioSource.Play();
    }

    //Decide which side the passed in room is on
    public void DecideRoom(RoomController room)
    {
        Transform closestSide = GetClosestSide(room.transform.position);

        if(closestSide == _sideA) _sideARoom = room;
        else _sideBRoom = room;
    }


    public Transform GetClosestSide(Vector3 pos)
    {
        if (_sideA == _sideB) return _sideA;

        return Vector3.Distance(_sideA.position, pos) < Vector3.Distance(_sideB.position, pos) ? _sideA : _sideB;
    }

    public Transform GetFurthestSide(Vector3 pos)
    {
        if (_sideA == _sideB) return _sideA;

        return Vector3.Distance(_sideA.position, pos) < Vector3.Distance(_sideB.position, pos) ? _sideB : _sideA;
    }

    public Transform GetOppositeSide(Transform side)
    {
        //If side is side A then return side B 
        return side == _sideA ? _sideB : _sideA;
    }

    public void InteractWithObject()
    {
        _currentState = !_currentState;

        //if the current state is not the original state then the object has changed state.
        if (_currentState != _originalState)
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

    public void ReturnToOriginalState()
    {
        if (!_originalState)
        {
            Close();
        }

        _changedStateRecently = false;
        _changedState = false;
        _currentState = _originalState;
    }

    public virtual void Open() {}
    public virtual void Close() {}
    public virtual void InteractAction() { }
    public virtual void DecideAnimation() { }
}
