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


    float _timeSinceLastSound;
    bool _heardSound;
    AIAgent _aiAgent;

    private void Awake()
    {
        _aiAgent = GetComponentInParent<AIAgent>();
    }

    private void FixedUpdate()
    {
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
        _heardSound = true;

        if(_timerBetweenIncrements > _timeBetweenIncrements)
        {
            _timerBetweenIncrements = 0.0f;

            heardValue = Mathf.Min(heardValue + val, 1.0f);

            if (heardValue >= 1.0f)
            {
                _heard = true;
                _aiAgent.SoundHeard();
                _aiAgent.PointOfSound = origin;

            }
        }
    }

    public void SubtractSound(float val)
    {
        heardValue = Mathf.Max(heardValue - val, 0.0f);

        if(heardValue < 1.0f)
        {
            _heard = false;
        }
        if(heardValue == 0.0f)
        {
            _heardSound = false;
            //_aiAgent.NoLongerHearingSound();
        }
    }
}
