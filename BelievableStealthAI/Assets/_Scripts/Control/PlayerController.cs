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
        [SerializeField] CanvasGroup _deathScreenGroup;

        public GameObject FollowCam { get { return _followCam; } }

        bool _isStanding = true;
        public bool IsStanding { get { return _isStanding; } set { _isStanding = value; } }

        List<Hitbox> _hitboxes;
        public List<Hitbox> Hitboxes { get => _hitboxes; }

        Container _nearbyContainer;
        Door _nearbyDoor;
        AIAgent _nearbyAgent;
        Bodybag _nearbyBodybag;
        Window _nearbyWindow;
        Lightswitch _nearbySwitch;

        bool _visible = true;
        bool _canMove = true;

        bool _carryingBodybag;
        public bool Visible { get => _visible; }
        public bool CanMove { get => _canMove; }
        public Container NearbyContainer { get => _nearbyContainer; set { _nearbyContainer = value; SetPrompt(); } }
        public Lightswitch NearbyLightswitch { get => _nearbySwitch; set { _nearbySwitch = value; SetPrompt(); } }

        public Window NearbyWindow { get => _nearbyWindow; set { _nearbyWindow = value; SetPrompt(); } }
        public Door NearbyDoor { get => _nearbyDoor; set { _nearbyDoor = value; SetPrompt(); } }

        public AIAgent NearbyAgent { get => _nearbyAgent; set { _nearbyAgent = value; SetPrompt(); } }

        public Bodybag NearbyBodybag { get => _nearbyBodybag; set { _nearbyBodybag = value; SetPrompt(); } }

        private void SetPrompt()
        {
            if (_carryingBodybag)
            {
                _uiGPrompt.gameObject.SetActive(true);
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

                if(_nearbyContainer.PlayerInside)
                {
                    _uiEPrompt.SetText("Get Out");
                }
                else
                {
                    _uiEPrompt.SetText("Hide");
                }
            }
            else if(_nearbyAgent)
            {
                _uiEPrompt.SetText("Assassinate");

                if(_nearbyAgent.GetComponent<Health>().IsDead)
                {
                    _uiEPrompt.SetText("Pickup");
                }
            }
            else if(_nearbyBodybag)
            {
                _uiEPrompt.SetText("Pickup");
            }
            else if(_nearbyWindow)
            {
                if (_nearbyWindow.CurrentState) _uiEPrompt.SetText("Close Window");
                else _uiEPrompt.SetText("Open Window");
            }
            else if(_nearbySwitch)
            {
                if (_nearbySwitch.CurrentState) _uiEPrompt.SetText("Turn off light");
                else _uiEPrompt.SetText("Turn on light");
            }
            else
            {
                _uiEPrompt.gameObject.SetActive(false);
            }
        }

        private void Awake()
        {
            _hitboxes = GetComponentsInChildren<Hitbox>().ToList();
        }

        private void Update()
        {
            bool stateChanged = true;
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
                            NearbyAgent = null;
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
            else if(_nearbyContainer)
            {
                if (!_carryingBodybag)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (_nearbyContainer.PlayerInside) HandleLocker(false);
                        else HandleLocker(true);
                    }

                    if(Input.GetKeyDown(KeyCode.G))
                    {
                        if (_nearbyContainer.BodybagInside)
                        {
                            _nearbyContainer.BodybagInside = false;
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
                                _nearbyContainer.BodybagInside = false;
                                SetCarryingBodybag(true);
                            }
                            else
                            {
                                //TODO: Tell player they cannot have 2 bodybags at the same time
                            }
                        }
                        else
                        {
                            _nearbyContainer.BodybagInside = true;
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
                        NearbyBodybag = null;
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
                }
            }
            else if(_nearbyWindow)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    _nearbyWindow.DecideAnimation();
                    _nearbyWindow.InteractWithObject();
                }
            }
            else if(_nearbySwitch)
            {
                if(Input.GetKeyDown(KeyCode.E))
                {
                    _nearbySwitch.InteractWithObject();
                    _nearbySwitch.HandleLogic();
                }
            }
            else if(_carryingBodybag)
            {
                if(Input.GetKeyDown(KeyCode.G))
                {
                    SetCarryingBodybag(false);

                    Bodybag bodybag = Instantiate(_bodybagPrefab);
                    Vector3 spawnPos = transform.position + (UnityEngine.Random.insideUnitSphere * 1.5f);
                    if (NavMesh.SamplePosition(spawnPos, out NavMeshHit hit, 3.0f, ~0))
                    {
                        spawnPos = hit.position;
                    }

                    bodybag.transform.position = spawnPos;
                }
            }
            else
            {
                stateChanged = false;
            }

            if (stateChanged) SetPrompt();
        }

        void HandleLocker(bool getInLocker)
        {
            _nearbyContainer.PlayerInside = getInLocker;
            _visible = !getInLocker;
            _canMove = !getInLocker;
            _visibleMeshes.SetActive(!getInLocker);
        }

        void SetCarryingBodybag(bool val)
        {
            _bodybagAttachment.SetActive(val);
            _carryingBodybag = val;
        }
        
        public void TakeHit()
        {
            //Kill player
            //GetComponent<PlayerHealth>().TakeDamage(10000.0f);
            //_deathScreenGroup.alpha = 1.0f;
        }
    }
}