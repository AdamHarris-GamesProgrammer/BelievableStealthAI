using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class Blackboard
{
    public Vector3 moveToPosition;
    public GameObject moveToObject;
    public AILocomotion _locomotion;
    public AIAgent _agent;
    public PlayerController _player;
}
