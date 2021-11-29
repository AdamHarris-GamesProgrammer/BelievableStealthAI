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

    bool _inside = false;

    bool _visible = false;
    public bool Visible { get => _visible; }

    private void Awake()
    {
        _fovController = GetComponentInParent<FOVController>();
        _player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inside = true;
            StartCoroutine("Raycast");
            
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void Update()
    {
    }

    IEnumerator Test()
    {
        for(int i = 0; i < 100; i++)
        {
            Debug.Log(i);

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Raycast()
    {
        Debug.Log("Raycast Method");

        while(_inside)
        {
            bool found = false;

            Debug.Log("Raycast Method");

            foreach (Hitbox hitbox in _player.Hitboxes)
            {
                Debug.Log("Testing against: " + hitbox.name);

                RaycastHit hit;
                Vector3 pos = hitbox.transform.position;
                Vector3 direction = (pos - _fovController.transform.position);
                Debug.DrawRay(_fovController.transform.position, direction);
                if (Physics.Raycast(_fovController.transform.position, direction, out hit, 25.0f, _rayCastLayer, QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.GetComponent<Hitbox>())
                    {
                        Debug.Log("Hit: " + hit.transform.name);
                        found = true;
                        _fovController.AddValue(_detectionInrement * hitbox.DetectionMultiplier);
                    }
                }

                yield return new WaitForEndOfFrame();
            }

            _visible = found;

            yield return new WaitForEndOfFrame();
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
    }

}
