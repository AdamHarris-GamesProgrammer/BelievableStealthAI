using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVController : MonoBehaviour
{
    [Min(0f)][SerializeField] private float _detectedThreshold = 1.0f;
    [SerializeField] private float _detectedValue;
    [SerializeField] bool _seen = false;

    float _lostVisibilityTimer = 0.0f;
    [SerializeField] float _lostVisibilityDuration = 0.5f;

    float _timeBetweenAditions;
    [SerializeField] float _timeBetweenDuration = 0.25f;

     FOVCollider[] _colliderArr;

    private void Awake()
    {
       _colliderArr = GetComponentsInChildren<FOVCollider>();
    }
    public void AddValue(float increment)
    {
        if(_timeBetweenAditions >= _timeBetweenDuration)
        {
            //Debug.Log("Adding: " + increment);
            _detectedValue = Mathf.Min(_detectedValue + increment, _detectedThreshold);

            if (_detectedValue > _detectedThreshold)
            {
                _seen = true;
            }

            _timeBetweenAditions = 0.0f;
        }
    }

    public void SubtractValue(float decrement)
    {
        _detectedValue = Mathf.Max(_detectedValue - decrement, 0f);

        if(_detectedValue < _detectedThreshold)
        {
            _seen = false;
        }
    }

    private void FixedUpdate()
    {
        _timeBetweenAditions += Time.fixedDeltaTime;
        //Debug.Log(string.Format("Detected: {0}", _seen));


        if(_detectedValue > 0.0f)
        {
            bool visible = false;
            foreach (FOVCollider col in _colliderArr)
            {
                if (col.Visible)
                {
                    visible = true;
                    _lostVisibilityTimer = 0.0f;
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
