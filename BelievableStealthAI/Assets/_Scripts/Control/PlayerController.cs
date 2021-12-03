using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace TGP.Control
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] GameObject _followCam;
        public GameObject FollowCam { get { return _followCam; } }

        List<AudioPerception> _audioPercievers;

        bool _isStanding = true;
        public bool IsStanding { get { return _isStanding; } set { _isStanding = value; } }

        List<Hitbox> _hitboxes;
        public List<Hitbox> Hitboxes { get => _hitboxes; }

        private void Awake()
        {
            _hitboxes = GetComponentsInChildren<Hitbox>().ToList();
            _audioPercievers = FindObjectsOfType<AudioPerception>().ToList();
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                ProduceSound(1.0f, 10.0f);
            }  
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                ProduceSound(1.0f, 20.0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ProduceSound(1.0f, 30.0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                ProduceSound(1.0f, 40.0f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                ProduceSound(1.0f, 50.0f);
            }
        }

        void ProduceSound(float val, float maxDistance)
        {
            Debug.Log("Producing Sound with value: " + val + " and maxDistance: " + maxDistance);

            foreach(AudioPerception perciever in _audioPercievers)
            {
                float totalDistance = 0.0f;

                NavMeshPath path = new NavMeshPath();
                if(NavMesh.CalculatePath(transform.position, perciever.transform.position, ~0, path))
                {
                    if (InRange(path, maxDistance, ref totalDistance))
                    {
                        Debug.Log("Perceiver will hear sound. Total Distance: " + totalDistance);
                        float percentageOfMaxDistance = totalDistance / maxDistance;
                        float heard = val * (1 - percentageOfMaxDistance);
                        perciever.AddSound(heard);
                    }
                    else
                    {
                        Debug.Log("Perceiver not in range. Max Distance: " + maxDistance);
                    }
                }
            }
        }


        bool InRange(NavMeshPath path, float maxDistance, ref float distance)
        {
            //TODO: Change to source position, for distraction objects.
            Vector3 position = transform.position;
            foreach(Vector3 corner in path.corners)
            {
                distance += Vector3.Distance(position, corner);
                position = corner;

                if(distance >= maxDistance)
                {
                    return false;
                }
            }

            return true;
        }
    }
}

