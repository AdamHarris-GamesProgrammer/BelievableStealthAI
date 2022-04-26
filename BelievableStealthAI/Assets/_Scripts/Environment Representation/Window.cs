using UnityEngine;

public class Window : ObservableObject
{
    //The window frame base needs their collider to be disabled to walk through it. No going through window animation is causing this
    [SerializeField] Collider _colliderToDisable;
    [SerializeField] bool _shouldBeOpen;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _player = FindObjectOfType<PlayerController>();

        if (_shouldBeOpen)
        {
            _animator.Play("WindowOpen", 0, 0.0f);
            _currentState = true;
        }

        _originalState = _currentState;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the enemy enters this trigger
        if (other.CompareTag("Enemy"))
        {
            //Sets the nearby observable to this. Used for interacting with the object
            other.GetComponent<AIAgent>().NearbyObservable = this;
        }
        //If the player enters this trigger
        else if (other.CompareTag("Player"))
        {
            //Sets the nearby window to this
            _player.NearbyWindow = this;
        }
    }

    public override void Open()
    {
        //Plays the window open animation and SFX
        _animator.Play("WindowOpen", 0, 0.0f);
        PlaySFX();
    }

    public override void Close()
    {
        //Plays the window close animation and SFX
        _animator.Play("WindowClose", 0, 0.0f);
        PlaySFX();
    }

    private void OnTriggerExit(Collider other)
    {
        //If the enemy leaves this trigger
        if (other.CompareTag("Enemy"))
        {
            //Get the AIAgent component
            AIAgent enemy = other.GetComponent<AIAgent>();

            //Sets the nearby observable to null
            enemy.NearbyObservable = null;

            //Get the closest side to the enemies position
            Transform closest = GetClosestSide(enemy.transform.position);

            //Selects the current room for the enemy 
            enemy.CurrentRoom = closest == _sideA ? _sideARoom : _sideBRoom;
        }
        //If the player leaves this trigger
        else if (other.CompareTag("Player"))
        {
            //Sets the players nearby window to null
            _player.NearbyWindow = null;
        }
    }

    public override void DecideAnimation()
    {
        //If open then close the window
        if (_currentState)
        {
            Close();
            //Enables the collider
            _colliderToDisable.enabled = true;
        }
        //if closed then open the window
        else
        {
            Open();
            //Disable the collider
            _colliderToDisable.enabled = false;
        }
    }
}
