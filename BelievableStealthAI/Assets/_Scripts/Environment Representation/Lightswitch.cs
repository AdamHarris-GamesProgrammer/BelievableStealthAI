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
        //Sets the light to on or off based on the current state
        _controlsLight.gameObject.GetComponent<Light>().enabled = _currentState;

        //Play sound effect
        PlaySFX();

        //Tell agents in room that a lightswitch has changed
        _room.AgentsInRoom.ForEach(agent => agent.LightSwitchChanged(this));
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the player enters this trigger then set the nearby lightswitch to this
        if (other.CompareTag("Player"))
        {
            _player.NearbyLightswitch = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //If the player leaves this trigger then set the nearby lightswitch to null
        if (other.CompareTag("Player"))
        {
            _player.NearbyLightswitch = null;
        }
    }
}
