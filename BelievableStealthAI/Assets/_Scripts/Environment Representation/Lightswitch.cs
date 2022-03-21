using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Lightswitch : ObservableObject
{
    [SerializeField] Light _controlsLight;

    [SerializeField] RoomController _room;

    public RoomController Room { get => _room; set => _room = value; }

    private void Awake()
    {
        _player = FindObjectOfType<PlayerController>();

        _currentState = _controlsLight.GetComponent<Light>().enabled;
        _originalState = _currentState;
    }

    public override void InteractAction()
    {
        _controlsLight.gameObject.GetComponent<Light>().enabled = _currentState;

        _controlsLight.gameObject.GetComponent<Light>().enabled = _currentState;

        PlaySFX();

        foreach (AIAgent agent in _room.AgentsInRoom)
        {
            agent.LightSwitchChanged(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.NearbyLightswitch = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _player.NearbyLightswitch = null;
        }
    }
}
