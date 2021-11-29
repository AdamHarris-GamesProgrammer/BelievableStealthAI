using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        bool _isShooting = false;
        public bool IsShooting { get { return _isShooting; } set { _isShooting = value; } }

        bool _isStanding = true;
        public bool IsStanding { get { return _isStanding; } set { _isStanding = value; } }


        bool _isStationary = false;
        public bool IsStationary { get { return _isStationary; } set { _isStationary = value; } }


        Animator _animator;

        List<Hitbox> _hitboxes;
        public List<Hitbox> Hitboxes { get => _hitboxes; }

        //Stores if we have been detected by the AI
        bool _detected = false;
        public bool IsDetected { get { return _detected; } set { _detected = value; } }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _hitboxes = GetComponentsInChildren<Hitbox>().ToList();
        }

        private void Update()
        {
        }

    }
}

