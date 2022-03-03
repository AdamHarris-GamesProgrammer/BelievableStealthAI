using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField] protected Transform _investigationPoint;

    protected bool _playerInside = false;
    protected bool _bodyBagInside = false;

    public bool PlayerInside { get => _playerInside; set { _playerInside = value; } }
    public bool BodybagInside { get => _bodyBagInside; set { _bodyBagInside = value; } }

    public Transform InvestigationPoint { get => _investigationPoint; }

    public RoomController Room { get => _room; set => _room = value; }
    [SerializeField] protected RoomController _room;
    public bool Search()
    {
        if (_playerInside || _bodyBagInside)
        {
            return true;
        }

        return false;
    }

}
