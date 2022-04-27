using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAgent : MonoBehaviour
{
    [SerializeField] KeyCode _key;
    [SerializeField] AIAgent _agentToKill;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            _agentToKill.GetComponent<Health>().Kill();
        }        
    }
}
