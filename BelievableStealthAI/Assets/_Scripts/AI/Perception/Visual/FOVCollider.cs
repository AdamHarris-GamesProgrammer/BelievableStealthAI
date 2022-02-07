using System.Collections;
using System.Collections.Generic;
using TGP.Control;
using UnityEngine;

public class FOVCollider : MonoBehaviour
{
    [Range(0f, 1f)][SerializeField] private float _detectionInrement = 0.1f;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inside = true;
            StartCoroutine("Raycast");
        }
        else if(other.CompareTag("ObservableObject"))
        {
            _observedObjects.Add(other.GetComponent<ObservableObject>());
        }
        else if(other.CompareTag("DeadBody"))
        {
            _bodiesInCollider.Add(other.gameObject);
        }
    }

    private void FixedUpdate()
    {
        //TODO: Switch this to notify the AI controller of the observed object rather than handle it here
        if (_observedObjects.Count > 0)
        {
            if(!_agent.HasAnObjectchanged)
            {
                foreach (ObservableObject obj in _observedObjects)
                {
                    RaycastHit hit;
                    Vector3 pos = obj.transform.position;
                    Vector3 direction = (pos - _fovController.RaycastOrigin);
                    Debug.DrawRay(_fovController.RaycastOrigin, direction);
                    if (Physics.Raycast(_fovController.RaycastOrigin, direction, out hit, 25.0f, _rayCastLayer, QueryTriggerInteraction.Ignore))
                    {
                        ObservableObject observable = hit.transform.GetComponentInParent<ObservableObject>();
                        if(observable != null)
                        {
                            if(observable.HasRecentlyChanged)
                            {
                                _agent.SeenChangedObject(observable);
                            }
                        }
                    }
                }
            }
        }

        if(_bodiesInCollider.Count > 0)
        {
            foreach(GameObject obj in _bodiesInCollider)
            {
                RaycastHit hit;
                Vector3 pos = obj.transform.position;
                Vector3 direction = (pos - _fovController.RaycastOrigin);
                Debug.DrawRay(_fovController.RaycastOrigin, direction);
                if (Physics.Raycast(_fovController.RaycastOrigin, direction, out hit, 25.0f, _rayCastLayer))
                {
                    if (hit.transform.GetInstanceID() == obj.transform.GetInstanceID())
                    {
                        _agent.BodyDetected();
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator Raycast()
    {
        while(_inside)
        {
            bool found = false;

            foreach (Hitbox hitbox in _player.Hitboxes)
            {
                //Debug.Log("Testing against: " + hitbox.name);

                RaycastHit hit;
                Vector3 pos = hitbox.transform.position;
                Vector3 direction = (pos - _fovController.RaycastOrigin);
                Debug.DrawRay(_fovController.RaycastOrigin, direction);
                if (Physics.Raycast(_fovController.RaycastOrigin, direction, out hit, 25.0f, _rayCastLayer, QueryTriggerInteraction.Ignore))
                {
                    //Debug.Log("Hit: " + hit.transform.gameObject.name);

                    if (hit.transform.GetComponent<Hitbox>())
                    {
                        if(_player.Visible)
                        {
                            //Debug.Log("Hit: " + hit.transform.name);
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inside = false;

            _visible = false;
            StopCoroutine("Raycast");
        }
        else if(other.CompareTag("ObservableObject"))
        {
            _observedObjects.Remove(other.GetComponent<ObservableObject>());
        }
        else if(other.CompareTag("DeadBody"))
        {
            _bodiesInCollider.Remove(other.gameObject);
        }
    }

}
