using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVCollider : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] private float _detectionInrement = 0.1f;
    [SerializeField] LayerMask _rayCastLayer;

    FOVController _fovController;



    PlayerController _player;
    AIAgent _agent;

    bool _inside = false;

    bool _visible = false;
    public bool Visible { get => _visible; }

    public List<ObservableObject> _observedObjects;
    public List<GameObject> _bodiesInCollider;

    private void Awake()
    {
        _fovController = GetComponentInParent<FOVController>();
        _player = FindObjectOfType<PlayerController>();
        _bodiesInCollider = new List<GameObject>();
        _agent = GetComponentInParent<AIAgent>();
    }

    private void Start()
    {
        StartCoroutine(RaycastToObservables());
        StartCoroutine(RaycastToPlayer());
    }

    private void FixedUpdate()
    {
        if(_bodiesInCollider.Count > 0)
        {
            if (!_agent.HasSeenBody)
            {
                foreach (GameObject obj in _bodiesInCollider)
                {
                    if (obj == null) continue;

                    RaycastHit hit;
                    Vector3 pos = obj.transform.position;
                    Vector3 direction = (pos - _fovController.RaycastOrigin);
                    //Debug.DrawRay(_fovController.RaycastOrigin, direction);
                    if (Physics.Raycast(_fovController.RaycastOrigin, direction, out hit, 25.0f, _rayCastLayer, QueryTriggerInteraction.Ignore))
                    {
                        if (hit.transform.GetInstanceID() == obj.transform.GetInstanceID())
                        {
                            _agent.BodyDetected(hit.transform.gameObject);
                            break;
                        }
                    }
                }
            }
        }
        
    }

    private IEnumerator RaycastToObservables()
    {
        while (true)
        {
            if (_agent.CurrentRoom != null)
            {
                if (!_agent.HasAnObjectchanged)
                {
                    //Cycle through each observable
                    foreach (ObservableObject obj in _agent.CurrentRoom.ObservablesInRoom)
                    {
                        //if the obj has not recently changed then don't perform the raycast
                        if (!obj.HasRecentlyChanged) continue;

                        //Calculate direction vector
                        Vector3 pos = obj.transform.position;
                        Vector3 direction = (pos - _fovController.RaycastOrigin);
                        Debug.DrawRay(_fovController.RaycastOrigin, direction);

                        RaycastHit hit;
                        if (Physics.Raycast(_fovController.RaycastOrigin, direction, out hit, 25.0f, _rayCastLayer, QueryTriggerInteraction.Ignore))
                        {
                            ObservableObject observable = hit.transform.GetComponentInParent<ObservableObject>();

                            //If we have an observable
                            if (observable)
                            {
                                //if the object has recently changed
                                if (observable.HasRecentlyChanged)
                                    //Tell the agent we have seen a changed object
                                    _agent.SeenChangedObject(observable);
                            }
                        }
                        //Wait for next frame
                        yield return new WaitForFixedUpdate();
                    }
                }
                //Wait for next frame
                yield return new WaitForFixedUpdate();
            }
            //Wait for next frame
            yield return new WaitForFixedUpdate();
        }
    }
       

    private IEnumerator RaycastToPlayer()
    {
        //repeat forever
        while (true)
        {
            //while the player is inside this collider
            while (_inside)
            {
                //cycle through each hitbox
                foreach (Hitbox hitbox in _player.Hitboxes)
                {
                    //Calculate the direction vector
                    Vector3 pos = hitbox.transform.position;
                    Vector3 direction = (pos - _fovController.RaycastOrigin).normalized;
                    Debug.DrawRay(_fovController.RaycastOrigin, direction);

                    //Try to raycast from the agents eyes to the player
                    RaycastHit hit;
                    if (Physics.Raycast(_fovController.RaycastOrigin, direction, out hit, 35.0f, _rayCastLayer, QueryTriggerInteraction.Ignore))
                    {
                        //if the hit is a player hitbox
                        if (hit.transform.CompareTag("PlayerHitbox"))
                        {
                            //if the player is visible (not in a locker)
                            if (_player.Visible)
                            {
                                _visible = true;
                                //Add a value to the fov controller
                                _fovController.AddValue(_detectionInrement * hitbox.DetectionMultiplier);
                            }
                        }
                    }
                    //Wait for next frame
                    yield return new WaitForEndOfFrame();
                }
                //Wait for next frame
                yield return new WaitForEndOfFrame();
            }
            //Wait for next frame
            _visible = false;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Allows the agent to raycast to player and dead bodies
        if (other.CompareTag("Player"))
        {
            _inside = true;
        }
        else if (other.CompareTag("DeadBody"))
        {
            _bodiesInCollider.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Stops the agent from raycasting to these objects
        if (other.CompareTag("Player"))
        {
            _inside = false;

            _visible = false;
        }
        else if (other.CompareTag("DeadBody"))
        {
            _bodiesInCollider.Remove(other.gameObject);
        }
    }
}
