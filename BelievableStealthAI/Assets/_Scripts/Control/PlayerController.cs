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
        [SerializeField] GameObject _visibleMeshes;
        [SerializeField] GameObject _uiEPrompt;
        [SerializeField] GameObject _bodybagAttachment;
        [SerializeField] GameObject _bodybagPrefab;

        public GameObject FollowCam { get { return _followCam; } }

        List<AudioPerception> _audioPercievers;

        bool _isStanding = true;
        public bool IsStanding { get { return _isStanding; } set { _isStanding = value; } }

        List<Hitbox> _hitboxes;
        public List<Hitbox> Hitboxes { get => _hitboxes; }

        Container _nearbyContainer;
        Door _nearbyDoor;
        AIAgent _nearbyAgent;

        bool _visible;
        bool _canMove = true;

        bool _carryingBodybag;
        public bool Visible { get => _visible; }
        public bool CanMove { get => _canMove; }

        public void SetContainer(Container container)
        {
            _nearbyContainer = container;
            SetPrompt();
        }

        public void SetDoor(Door door)
        {
            _nearbyDoor = door;
            SetPrompt();
        }

        public void SetAgent(AIAgent agent)
        {
            _nearbyAgent = agent;
            SetPrompt();
        }

        private void SetPrompt()
        {
            if(_nearbyDoor == null && _nearbyContainer == null && _nearbyAgent == null)
            {
                _uiEPrompt.SetActive(false);
            }
            else
            {
                _uiEPrompt.SetActive(true);
            }
        }

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


            if(_nearbyAgent)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if (_nearbyAgent.GetComponent<Health>().IsDead)
                    {
                        if (!_carryingBodybag)
                        {
                            _carryingBodybag = true;
                            _bodybagAttachment.SetActive(true);

                            //TODO: Make sure all edge cases are handled when destroying ai agent
                            Destroy(_nearbyAgent.gameObject);
                            SetAgent(null);
                        }
                    }
                    else
                    {
                        if (!_isStanding && !_nearbyAgent.CurrentlyAlert)
                        {
                            _nearbyAgent.GetComponent<Animator>().SetTrigger("stealthAssassinate");
                            _nearbyAgent.GetComponent<Health>().Kill();

                            //TODO: Play Assassinate animation
                            //TODO: Disable visual and auditory perception
                        }
                    }
                }
            }

            if(_nearbyContainer)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    if(_nearbyContainer.PlayerInside)
                    {
                        _nearbyContainer.GetOut();
                        _visible = true;
                        _canMove = true;
                        _visibleMeshes.SetActive(true);
                    }
                    else
                    {
                        _nearbyContainer.GetIn();
                        _visible = false;
                        _canMove = false;
                        _visibleMeshes.SetActive(false);
                    }
                }
            }

            if(_nearbyDoor)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    _nearbyDoor.DecideAnimation();
                    _nearbyDoor.InteractWithObject();
                }
            }

            if(_carryingBodybag)
            {
                if(Input.GetKeyDown(KeyCode.G))
                {
                    _carryingBodybag = false;
                    _bodybagAttachment.SetActive(false);

                    GameObject bodybag = Instantiate(_bodybagPrefab);

                    Vector3 spawnPos = transform.position + (UnityEngine.Random.insideUnitSphere * 1.5f);

                    NavMeshHit hit;
                    if(NavMesh.SamplePosition(spawnPos, out hit, 3.0f, ~0))
                    {
                        spawnPos = hit.position;
                    }

                    bodybag.transform.position = spawnPos;
                }
            }
        }

        //TODO: Take this into a audio producer class
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

