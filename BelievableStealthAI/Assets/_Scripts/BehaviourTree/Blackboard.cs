using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Blackboard
{
    public Vector3 moveToPosition;

    public Vector3 spawnPosition;
    public Vector3 spawnOrientation;

    public Transform closestInvestigationSide;
    public Transform furthestInvestigationSide;

    public bool _hasPatrolRoute = false;

    public float _walkSpeed = 1.5f;
    public float _patrolSpeed = 1.2f;
    public float _chaseSpeed = 5.4f;

    public Health _health;

    public List<PointOfInterest> _nearbyPointsOfInterest;
    public PointOfInterest _currentPOI;

    public GameObject moveToObject;
    public AILocomotion _locomotion;
    public AIAgent _agent;
    public PlayerController _player;

    public ObservableObject _changedObservedObject;

    public Transform _currentLookPoint;
    public List<Transform> _lookPoints;
}
