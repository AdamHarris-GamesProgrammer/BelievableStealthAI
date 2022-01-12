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

    public bool _hasPatrolRoute = false;

    public float _walkSpeed = 1.5f;
    public float _patrolSpeed = 1.2f;
    public float _chaseSpeed = 5.4f;


    public GameObject moveToObject;
    public AILocomotion _locomotion;
    public AIAgent _agent;
    public PlayerController _player;
}
