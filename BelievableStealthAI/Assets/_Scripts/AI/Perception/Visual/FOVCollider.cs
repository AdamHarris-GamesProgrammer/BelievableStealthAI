using System.Collections;
using System.Collections.Generic;
using TGP.Control;
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
        _observedObjects = new List<ObservableObject>();
        _agent = GetComponentInParent<AIAgent>();
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

                    //yield return new WaitForFixedUpdate();
                }
            }
        }
        
    }

    private IEnumerator RaycastToObservables()
    {
        if (!_agent.HasAnObjectchanged)
        {
            foreach (ObservableObject obj in _agent.CurrentRoom.ObservablesInRoom)
            {
                RaycastHit hit;
                Vector3 pos = obj.transform.position;
                Vector3 direction = (pos - _fovController.RaycastOrigin);
                //Debug.DrawRay(_fovController.RaycastOrigin, direction);
                if (Physics.Raycast(_fovController.RaycastOrigin, direction, out hit, 25.0f, _rayCastLayer, QueryTriggerInteraction.Ignore))
                {
                    ObservableObject observable = hit.transform.GetComponentInParent<ObservableObject>();

                    if (observable)
                    {
                        if(observable.HasRecentlyChanged)
                            _agent.SeenChangedObject(observable);
                    }
                }

                yield return new WaitForFixedUpdate();
            }
        }
        yield return new WaitForFixedUpdate();
    }

    private IEnumerator RaycastToPlayer()
    {
        while (_inside)
        {
            bool found = false;

            foreach (Hitbox hitbox in _player.Hitboxes)
            {
                RaycastHit hit;
                Vector3 pos = hitbox.transform.position;
                Vector3 direction = (pos - _fovController.RaycastOrigin).normalized;
                Debug.DrawRay(_fovController.RaycastOrigin, direction);
                if (Physics.Raycast(_fovController.RaycastOrigin, direction, out hit, 35.0f, _rayCastLayer, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.CompareTag("PlayerHitbox"))
                    {
                        if (_player.Visible)
                        {
                            found = true;
                            _fovController.AddValue(_detectionInrement * hitbox.DetectionMultiplier);
                        }
                    }
                }
                yield return new WaitForFixedUpdate();
            }

            _visible = found;

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inside = true;
            StartCoroutine(RaycastToPlayer());
        }
        else if (other.CompareTag("ObservableObject"))
        {
            //_observedObjects.Add(other.GetComponent<ObservableObject>());
            StartCoroutine(RaycastToObservables());
        }
        else if (other.CompareTag("DeadBody"))
        {
            _bodiesInCollider.Add(other.gameObject);
            //StartCoroutine(RaycastToBodies());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inside = false;

            _visible = false;
            StopCoroutine(RaycastToPlayer());
        }
        else if (other.CompareTag("ObservableObject"))
        {
           //_observedObjects.Remove(other.GetComponent<ObservableObject>());

            if(_observedObjects.Count == 0)
            {
                StopCoroutine(RaycastToObservables());
            }
        }
        else if (other.CompareTag("DeadBody"))
        {
            _bodiesInCollider.Remove(other.gameObject);
        }
    }
}
