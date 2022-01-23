using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField] Transform _investigationPoint;

    public Transform InvestigationPoint { get => _investigationPoint; }
    public virtual bool Search()
    {
        return false;
    }

}
