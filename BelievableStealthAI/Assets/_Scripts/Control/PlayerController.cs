using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace TGP.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] GameObject _aimCam;
        public GameObject AimCam { get { return _aimCam; } }
        [SerializeField] GameObject _followCam;
        public GameObject FollowCam { get { return _followCam; } }

        bool _inKillAnimation = false;

        public bool InKillAnimation { get { return _inKillAnimation; } }

        private GameObject _chestInventory;
        public GameObject ChestInventory { get { return _chestInventory; } }


        bool _isShooting = false;
        public bool IsShooting { get { return _isShooting; } set { _isShooting = value; } }

        bool _isStanding = true;
        public bool IsStanding { get { return _isStanding; } set { _isStanding = value; } }


        bool _isStationary = false;
        public bool IsStationary { get { return _isStationary; } set { _isStationary = value; } }

        private float _currency = 0.0f;
        public float Cash { get { return _currency; } }
        private int _roaches = 0;


        private bool _inInventory = false;
        public bool InInventory { get { return _inInventory; } set { _inInventory = value; } }

        Animator _animator;




        //Stores if we have been detected by the AI
        bool _detected = false;
        public bool IsDetected { get { return _detected; } set { _detected = value; } }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
        }

    }
}

