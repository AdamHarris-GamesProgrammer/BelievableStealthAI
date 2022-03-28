using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistractionObject : MonoBehaviour
{
    bool _activated = false;

    [SerializeField] float _distractionRadius = 15.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (_activated) return;

        GetComponent<AudioSource>().Play();

        _activated = true;

        AudioPerception perc = AudioProducer.PathToPercievers(transform.position, _distractionRadius);

        if(perc != null)
        {
            perc.GetComponentInParent<AIAgent>().Distracted(transform.position);
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.angularVelocity /= 5.0f;
        rb.velocity /= 5.0f;

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.Play();

        Destroy(rb, 5.0f);
        Destroy(ps, 5.0f);
        Destroy(this, 15.0f);
    }
}
