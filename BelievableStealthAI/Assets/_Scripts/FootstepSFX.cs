using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSFX : MonoBehaviour
{
    [SerializeField] AudioSource _source;

    //Called by Animation event
    public void PlayFootstep()
    {
        _source.Play();
    }
}
