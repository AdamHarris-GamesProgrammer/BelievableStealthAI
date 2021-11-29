using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Health _health;

    //The amount of damage a shot to this limb does
    [SerializeField] private float _damageMultiplier = 1.0f;
    [Range(0f,1f)][SerializeField] float _detectionMultiplier = 0.1f;


    public float DetectionMultiplier { get => _damageMultiplier; }

    void Awake()
    {
        //Gets this hitboxes owner's health component
        _health = GetComponentInParent<Health>();
    }
    

}
