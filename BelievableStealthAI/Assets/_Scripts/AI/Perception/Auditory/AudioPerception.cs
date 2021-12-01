using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPerception : MonoBehaviour
{
    [SerializeField] float heardValue = 0.0f;
    [SerializeField] bool _heard = false;

    public void AddSound(float val)
    {
        Debug.Log("Perciever heard sound with value: " + val);

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
    }


}
