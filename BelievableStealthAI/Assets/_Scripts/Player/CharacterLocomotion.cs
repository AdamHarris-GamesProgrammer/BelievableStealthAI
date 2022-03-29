using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.Animations.Rigging;


/*
 Sprint Animation
 produce different levels of noise while moving in crouch, walk or sprint
 play footstep sounds
 */
public class CharacterLocomotion : MonoBehaviour
{
    [Min(0f)] [SerializeField] private float _jumpHeight = 0.5f;
    [Min(0f)] [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _stepDown = 0.1f;
    [SerializeField] private float _walkSpeed = 1.0f;
    [SerializeField] private float _sprintSpeed = 1.8f;
    [SerializeField] private float _pushPower = 2.0F;


    private float _currentSpeed = 1.0f;


    AudioSource _audioSource;

    bool _isCrouching = false;
    bool _isSprinting = false;

    public bool IsCrouching { get { return _isCrouching; } }

    Vector2 _input;
    Vector3 _velocity;
    Vector3 _rootMotion;


    PlayerController _player;
    CharacterController _controller;
    Animator _animator;
    PlayerHealth _health;

    #region UNITY MESSAGES
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<PlayerHealth>();
        _player = GetComponent<PlayerController>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!_player.Started || _player.Won)
        {
            _animator.SetFloat("InputX", 0.0f);
            _animator.SetFloat("InputY", 0.0f);
            return;
        }
        if (_health.IsDead) return;

        float noiseLevel = 0.0f;
        float noiseDistance = 0.0f;

        if (!_player.CanMove)
        {
            _animator.SetFloat("InputX", 0.0f);
            _animator.SetFloat("InputY", 0.0f);
            return;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _isSprinting = true;
            _currentSpeed = _sprintSpeed;
            _animator.SetBool("isSprinting", true);
            noiseLevel = 0.45f;
            noiseDistance = 25.0f;

            if (_isCrouching)
            {
                _isCrouching = false;
                _animator.SetBool("isCrouching", false);
                _controller.height = 1.6f;
            }
        }
        else
        {
            noiseLevel = 0.15f;
            noiseDistance = 10.0f;
            _isSprinting = false;
            _currentSpeed = _walkSpeed;
            _animator.SetBool("isSprinting", false);
        }


        //Disables crouching if we are crouching enables crouching if I am standing
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCrouching = !_isCrouching;

            _controller.height = _isCrouching ? 1.5f : 1.6f;

            noiseLevel = 0.05f;
            noiseDistance = 5.0f;

            if (_isSprinting)
            {
                _isSprinting = false;
                _animator.SetBool("isSprinting", false);
            }
        }

        //Checks we are crouching and sets the bool in the animator
        if (_isCrouching) _animator.SetBool("isCrouching", true);
        else _animator.SetBool("isCrouching", false);

        //Handles our root motion
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        //Sets our animator values
        _animator.SetFloat("InputX", _input.x);
        _animator.SetFloat("InputY", _input.y);

        if (Mathf.Approximately(_input.x, 0.0f) && Mathf.Approximately(_input.y, 0.0f))
        {
            noiseLevel = 0.0f;
            noiseDistance = 0.0f;
        }

        if (noiseLevel != 0.0f)
        {
            AudioProducer.ProduceSound(transform.position, noiseLevel, noiseDistance);
        }

        //if we are crouching then we are not standing etc.
        _player.IsStanding = !_isCrouching;
    }
    private void FixedUpdate()
    {
        if (_health.IsDead) return;
        if (!_player.CanMove) return;

        UpdateOnGround();
    }

    private void OnAnimatorMove()
    {
        if (_health.IsDead) return;

        //Accumulates our root motion this frame
        _rootMotion += _animator.deltaPosition;
        //Debug.Log(_rootMotion);
    }

    #endregion

    #region PRIVATE METHODS

    private void UpdateOnGround()
    {
        //Moving along X and Z, and then scale move speed by the players move speed stat. (Modified by equipped armor)
        Vector3 stepForward = (_rootMotion * _currentSpeed);
        Vector3 stepDown = Vector3.down * _stepDown;

        _controller.Move(stepForward + stepDown);

        //To remove the one frame glitch check if we are no longer grounded here and then step back up
        if (!_controller.isGrounded) _controller.Move(Vector3.up * _stepDown);

        _rootMotion = Vector3.zero;

        //Stepped off edge
        if (!_controller.isGrounded) InheritVelocity(0.0f);
    }

    private void InheritVelocity(float jumpVelocity)
    {
        _velocity = _animator.velocity * 0.1f * _currentSpeed;
        _velocity.y = jumpVelocity;
    }

    #endregion
}
