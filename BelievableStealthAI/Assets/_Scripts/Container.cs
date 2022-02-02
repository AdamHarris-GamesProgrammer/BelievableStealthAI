using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class Container : PointOfInterest
{
    bool _playerInside = false;
    bool _bodyBagInside = false;

    public bool PlayerInside { get => _playerInside; set { _playerInside = value; } }
    public bool BodybagInside { get => _bodyBagInside; set { _bodyBagInside = value; } }

    public RoomController Room { get => _room; set => _room = value; }

    PlayerController _player;

    [SerializeField] RoomController _room;

    void Awake()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            _player.NearbyContainer = this;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            _player.NearbyContainer = null;
    }

    public override bool Search()
    {
        if(_playerInside || _bodyBagInside)
        {
            return true;
        }

        return false;
    }
}
