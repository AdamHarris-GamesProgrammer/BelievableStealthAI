using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FOVController : MonoBehaviour
{
    [Min(0f)] [SerializeField] private float _detectedThreshold = 1.0f;
    [SerializeField] private float _detectedValue;

    [SerializeField] bool _halfwaySeen = false;
    [SerializeField] bool _seen = false;


    Vector3 _raycastOrigin;

    public Vector3 RaycastOrigin { get => _raycastOrigin; }

    float _lostVisibilityTimer = 0.0f;
    [SerializeField] float _lostVisibilityDuration = 0.5f;

    float _timeBetweenAditions;
    [SerializeField] float _timeBetweenIncrements = 0.25f;

    [Header("UI Test")]
    [SerializeField] Image _detectionMeter;
    public Image DetectionMeter { get => _detectionMeter; }


    public float Multiplier { get => _multiplier; set => _multiplier = value; }
    float _multiplier = 1.0f;

    FOVCollider[] _colliderArr;

    AIAgent _aiController;
    PlayerController _player;

    Health _health;

    private void Awake()
    {
        _colliderArr = GetComponentsInChildren<FOVCollider>();
        _aiController = GetComponentInParent<AIAgent>();
        _player = FindObjectOfType<PlayerController>();
        _health = GetComponentInParent<Health>();
    }

    public void AddValue(float increment)
    {
        //if the ai is dead
        if (_health.IsDead) return;

        //if the time since the last addition is greater than the threshold
        if (_timeBetweenAditions >= _timeBetweenIncrements)
        {
            //Calculate the new detected value
            _detectedValue = Mathf.Min(_detectedValue + (increment * _multiplier), _detectedThreshold);

            //if the detected value is above the threshold
            if (_detectedValue >= _detectedThreshold)
            {
                _seen = true;
                _aiController.PlayerSeen();
            }
            //if the detected value is above the halfway detected threshold
            else if(_detectedValue >= _detectedThreshold / 2.0f)
            {
                _halfwaySeen = true;
                _aiController.PlayerHalfwaySeen(_player.transform.position);
            }
            _timeBetweenAditions = 0.0f;
            
            //Fill the detection meter
            _detectionMeter.fillAmount = _detectedValue;
        }
    }
    public void SubtractValue(float decrement)
    {
        Debug.Log("Subtracting value");

        if (_health.IsDead) return;

        _detectedValue = Mathf.Max(_detectedValue - decrement, 0f);

        if (_detectedValue < _detectedThreshold)
        {
            _seen = false;
            
        }
        if(_detectedValue < 0.5f)
        {
            _aiController.LostSightOfPlayer();
        }

        _detectionMeter.fillAmount = _detectedValue;
    }

    private void FixedUpdate()
    {
        if (_health.IsDead) return;

        _raycastOrigin = transform.position + (transform.forward * 0.1f) + (Vector3.up * 1.8f);

        _timeBetweenAditions += Time.fixedDeltaTime;

        if (_detectedValue > 0.0f)
        {
            bool visible = false;
            foreach (FOVCollider col in _colliderArr)
            {
                if (col.Visible)
                {
                    visible = true;
                    _lostVisibilityTimer = 0.0f;
                    _aiController.LastKnownPlayerPosition = _player.transform.position;
                    break;
                }
            }

            if (!visible)
            {
                _lostVisibilityTimer += Time.fixedDeltaTime;

                if (_lostVisibilityTimer > _lostVisibilityDuration)
                {
                    _lostVisibilityTimer = 0.0f;
                    SubtractValue(0.1f);
                }
            }
        }
    }

}
