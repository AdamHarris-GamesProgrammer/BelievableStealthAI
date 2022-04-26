using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPerception : MonoBehaviour
{
    [SerializeField] float heardValue = 0.0f;
    [SerializeField] bool _heard = false;
    [SerializeField] float _timeBeforeReducingValue = 1.0f;

    [SerializeField] float _timeBetweenIncrements = 0.5f;
    float _timerBetweenIncrements = 0.0f;


    public float Multiplier { get => _multiplier; set => _multiplier = value; }
    float _multiplier = 1.0f;

    float _timeSinceLastSound;
    bool _heardSound;
    AIAgent _aiAgent;
    Health _health;

    private void Awake()
    {
        _aiAgent = GetComponentInParent<AIAgent>();
        _health = GetComponentInParent<Health>();
    }

    private void FixedUpdate()
    {
        if (_health.IsDead) return;

        _timerBetweenIncrements += Time.fixedDeltaTime;
        if(_heardSound)
        {
            _timeSinceLastSound += Time.fixedDeltaTime;

            if(_timeSinceLastSound > _timeBeforeReducingValue)
            {
                SubtractSound(0.05f);
                _timeSinceLastSound = 0.0f;
            }
        }
    }

    public void AddSound(Vector3 origin, float val)
    {
        if (_health.IsDead) return;

        _heardSound = true;

        //if the time since the last increment is greater than the time between threshold
        if(_timerBetweenIncrements > _timeBetweenIncrements)
        {
            _timerBetweenIncrements = 0.0f;

            //Calculate the new heard value
            heardValue = Mathf.Min(heardValue + (val * _multiplier), 1.0f);

            //if the heard value is equal to one
            if (heardValue == 1.0f)
            {
                //Agent hears the sound
                _heard = true;
                _aiAgent.SoundHeard();
                _aiAgent.PointOfSound = origin;

            }
        }
    }
    public void SubtractSound(float val)
    {
        //Calculates the new heard value
        heardValue = Mathf.Max(heardValue - val, 0.0f);

        if(heardValue < 1.0f) _heard = false;
        if(heardValue == 0.0f) _heardSound = false;
    }
}
