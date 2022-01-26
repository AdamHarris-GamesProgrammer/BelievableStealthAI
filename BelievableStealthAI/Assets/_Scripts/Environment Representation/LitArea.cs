using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitArea : MonoBehaviour
{
    Lightswitch _switch;

    private void Awake()
    {
        _switch = GetComponentInParent<Lightswitch>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AIAgent agent = other.gameObject.GetComponent<AIAgent>();
            _switch.AddAgent(agent);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            AIAgent agent = other.gameObject.GetComponent<AIAgent>();
            _switch.RemoveAgent(agent);
        }
    }
}
