using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPerception : MonoBehaviour
{
    [SerializeField] float heardValue = 0.0f;
    [SerializeField] bool _heard = false;
    [SerializeField] float _timeBeforeReducingValue = 1.0f;

    float _timeSinceLastSound;
    bool _heardSound;

    private void FixedUpdate()
    {
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

    public void AddSound(float val)
    {
        Debug.Log("Perciever heard sound with value: " + val);
        _heardSound = true;

        heardValue = Mathf.Min(heardValue + val, 1.0f);

        if(heardValue >= 1.0f)
        {
            _heard = true;
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
        }
    }
}