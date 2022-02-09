using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField] protected Transform _investigationPoint;

    public Transform InvestigationPoint { get => _investigationPoint; }

    public RoomController Room { get => _room; set => _room = value; }
    [SerializeField] protected RoomController _room;
    public virtual bool Search()
    {
        return false;
    }

}
