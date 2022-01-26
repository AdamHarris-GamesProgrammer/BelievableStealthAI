using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Lightswitch : ObservableObject
{
    [SerializeField] Light _controlsLight;

    PlayerController _controller;

    private void Awake()
    {
        _controller = FindObjectOfType<PlayerController>();

        if(_controlsLight.gameObject.GetComponent<Light>().enabled)
        {
            InteractWithObject();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _controller.NearbyLightswitch = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _controller.NearbyLightswitch = null;
        }
    }

    public void HandleLogic()
    {
        _controlsLight.gameObject.GetComponent<Light>().enabled = _currentState;
    }
}
