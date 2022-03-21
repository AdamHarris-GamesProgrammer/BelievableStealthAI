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

        List<AIAgent> agents = FindObjectsOfType<AIAgent>().ToList();

        AIAgent closestAgent = null;
        float currentBest = 1000.0f;
        foreach (AIAgent agent in agents) 
        {
            float dist = Vector3.Distance(agent.transform.position, transform.position);
            if (dist > _distractionRadius) continue;

            if (dist < currentBest)
            {
                currentBest = dist;
                closestAgent = agent;
            }
        }

        if(closestAgent != null)
        {
            closestAgent.Distracted(collision.GetContact(0).point);
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
