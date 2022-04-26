using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for the container class. Could be expanded further for other types of POI
public class PointOfInterest : MonoBehaviour
{
    [SerializeField] protected Transform _investigationPoint;

    //Set to true when a player hides in the POI.
    protected bool _playerInside = false;

    //Set to true when a bodybag is hidden in here
    protected bool _bodyBagInside = false;

    public bool PlayerInside { get => _playerInside; set { _playerInside = value; } }
    public bool BodybagInside { get => _bodyBagInside; set { _bodyBagInside = value; } }

    //Returns the investigation point for this object
    public Transform InvestigationPoint { get => _investigationPoint; }

    //Gets and sets the room that this object is in
    public RoomController Room { get => _room; set => _room = value; }

    //The room the object is in needs to be saved in the editor so this is marked as a serialized field
    [SerializeField] protected RoomController _room;

    //Returns true is the player or a bodybag is inside this object. Returns false if neither is the case
    public bool Search()
    {
        if (_playerInside || _bodyBagInside)
        {
            return true;
        }

        return false;
    }

}
