using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DistractionObject : MonoBehaviour
{
    bool _activated = false;

    [SerializeField] float distractionRadius = 15.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (_activated) return;

        _activated = true;

        List<AIAgent> agents = FindObjectsOfType<AIAgent>().ToList();

        AIAgent closestAgent = null;
        float currentBest = 1000.0f;
        foreach (AIAgent agent in agents) 
        {
            float dist = Vector3.Distance(agent.transform.position, transform.position);
            if (dist > distractionRadius) continue;

            if (dist < currentBest)
            {
                currentBest = dist;
                closestAgent = agent;
            }
        }

        if(closestAgent != null)
        {
            Debug.Log("Closest AI is: " + closestAgent.transform.name);
            //TODO: Trigger distracted stuff in AIAgent
        }

        Destroy(this, 15.0f);
    }
}
