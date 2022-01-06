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
        [SerializeField] UIPrompt _uiEPrompt;
        [SerializeField] UIPrompt _uiGPrompt;
        [SerializeField] GameObject _bodybagAttachment;
        [SerializeField] Bodybag _bodybagPrefab;

        public GameObject FollowCam { get { return _followCam; } }

        List<AudioPerception> _audioPercievers;

        bool _isStanding = true;
        public bool IsStanding { get { return _isStanding; } set { _isStanding = value; } }

        List<Hitbox> _hitboxes;
        public List<Hitbox> Hitboxes { get => _hitboxes; }

        Container _nearbyContainer;
        Door _nearbyDoor;
        AIAgent _nearbyAgent;
        Bodybag _nearbyBodybag;

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

        public void SetBodybag(Bodybag bodybag)
        {
            _nearbyBodybag = bodybag;
            SetPrompt();
        }

        private void SetPrompt()
        {
            if (_carryingBodybag)
            {
                _uiGPrompt.SetText("Drop Bodybag");
            }
            else
            {
                _uiGPrompt.gameObject.SetActive(false);
            }

            _uiEPrompt.gameObject.SetActive(true);
            if (_nearbyDoor)
            {
                if (_nearbyDoor.CurrentState) _uiEPrompt.SetText("Close Door");
                else _uiEPrompt.SetText("Open Door");
            }
            else if (_nearbyContainer)
            {
                if (_nearbyContainer.BodybagInside)
                {
                    _uiEPrompt.gameObject.SetActive(false);
                    _uiGPrompt.gameObject.SetActive(true);
                    _uiGPrompt.SetText("Unhide Bodybag");
                }
                else
                {
                    _uiEPrompt.SetText("Hide");

                    if(_carryingBodybag)
                    {
                        _uiGPrompt.SetText("Hide bodybag");
                        _uiGPrompt.gameObject.SetActive(true);
                    }
                }
            }
            else if(_nearbyAgent)
            {
                _uiEPrompt.SetText("Assassinate");
            }
            else if(_nearbyBodybag)
            {
                _uiEPrompt.SetText("Pickup");
            }
            else
            {
                _uiEPrompt.gameObject.SetActive(false);
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
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                ProduceSound(1.0f, 30.0f);
            }
            if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                ProduceSound(1.0f, 40.0f);
            }
            if(Input.GetKeyDown(KeyCode.Alpha5))
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
                            SetCarryingBodybag(true);

                            //TODO: Make sure all edge cases are handled when destroying ai agent
                            Destroy(_nearbyAgent.gameObject);
                            SetAgent(null);
                            _uiGPrompt.gameObject.SetActive(true);
                            _uiGPrompt.SetText("Drop");
                        }
                    }
                    else
                    {
                        if (!_isStanding && !_nearbyAgent.CurrentlyAlert)
                        {
                            _nearbyAgent.GetComponent<Animator>().SetTrigger("stealthAssassinate");
                            _nearbyAgent.GetComponent<Health>().Kill();
                            _uiEPrompt.gameObject.SetActive(true);
                            _uiEPrompt.SetText("Pickup");
                            //TODO: Play Assassinate animation
                            //TODO: Disable visual and auditory perception
                        }
                    }
                }
            }

            else if(_nearbyContainer)
            {
                if (!_carryingBodybag)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (_nearbyContainer.PlayerInside)
                        {
                            _nearbyContainer.GetOut();
                            _uiEPrompt.SetText("Hide");
                            _visible = true;
                            _canMove = true;
                            _visibleMeshes.SetActive(true);
                        }
                        else
                        {
                            _nearbyContainer.GetIn();
                            _uiEPrompt.SetText("Get Out");
                            _visible = false;
                            _canMove = false;
                            _visibleMeshes.SetActive(false);
                        }
                    }

                    if(Input.GetKeyDown(KeyCode.G))
                    {
                        if (_nearbyContainer.BodybagInside)
                        {
                            _nearbyContainer.UnhideBodybag();
                            _uiGPrompt.SetText("Hide Bodybag");
                            SetCarryingBodybag(true);
                        }
                    }
                }
                else
                {
                    if(Input.GetKeyDown(KeyCode.G))
                    {
                        if(_nearbyContainer.BodybagInside)
                        {
                            if(!_carryingBodybag)
                            {
                                _nearbyContainer.UnhideBodybag();
                                _uiGPrompt.SetText("Hide Bodybag");
                                _uiGPrompt.gameObject.SetActive(true);
                                SetCarryingBodybag(true);
                            }
                            else
                            {
                                //TODO: Tell player they cannot have 2 bodybags at the same time
                            }
                        }
                        else
                        {
                            _nearbyContainer.HideBoybag();
                            _uiGPrompt.SetText("Unhide Bodybag");
                            _uiGPrompt.gameObject.SetActive(true);
                            SetCarryingBodybag(false);
                        }
                    }
                }


            }

            else if(_nearbyBodybag)
            {
                if(!_carryingBodybag)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        SetCarryingBodybag(true);
                        //TODO: Make sure all edge cases are handled when destroying ai agent
                        Destroy(_nearbyBodybag.gameObject);
                        SetBodybag(null);
                        _uiGPrompt.gameObject.SetActive(true);
                        _uiGPrompt.SetText("Drop Bodybag");
                    }
                }
                else
                {
                    //TODO: Tell player they cannot carry multiple body bags
                }
            }

            else if(_nearbyDoor)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    _nearbyDoor.DecideAnimation();
                    _nearbyDoor.InteractWithObject();

                    if (_nearbyDoor.CurrentState) _uiEPrompt.SetText("Close Door");
                    else _uiEPrompt.SetText("Open Door");
                }
            }

            else if(_carryingBodybag)
            {
                if(Input.GetKeyDown(KeyCode.G))
                {
                    SetCarryingBodybag(false);
                    _uiGPrompt.gameObject.SetActive(false);

                    Bodybag bodybag = Instantiate(_bodybagPrefab);
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

        void SetCarryingBodybag(bool val)
        {
            _bodybagAttachment.SetActive(val);
            _carryingBodybag = val;
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