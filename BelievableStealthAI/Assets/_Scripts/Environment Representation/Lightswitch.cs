using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Lightswitch : ObservableObject
{
    [SerializeField] Light _controlsLight;

    PlayerController _controller;

    RoomController _room;

    public RoomController Room { get => _room; set => _room = value; }

    private void Awake()
    {
        _controller = FindObjectOfType<PlayerController>();

        if (_controlsLight.gameObject.GetComponent<Light>().enabled)
        {
            InteractWithObject();
        }
    }

    public override void InteractAction()
    {
        _controlsLight.gameObject.GetComponent<Light>().enabled = _currentState;
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
        if (other.CompareTag("Player"))
        {
            _controller.NearbyLightswitch = null;
        }
    }

    public void HandleLogic()
    {
        _controlsLight.gameObject.GetComponent<Light>().enabled = _currentState;

        foreach (AIAgent agent in _room.AgentsInRoom)
        {
            agent.LightSwitchChanged(this);
        }
    }
}
