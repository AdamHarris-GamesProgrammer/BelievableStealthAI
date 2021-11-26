using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVCollider : MonoBehaviour
{
    [Range(0f, 1f)][SerializeField] private float _detectionInrement = 0.1f;

    FOVController _fovController;

    bool _inside = false;

    bool _visible = false;
    public bool Visible { get => _visible; }

    private void Awake()
    {
        _fovController = GetComponentInParent<FOVController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inside = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(_inside)
        {
            _fovController.AddValue(_detectionInrement);

            //TODO: Set this through a ray cast
            _visible = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inside = false;

            _visible = false;
        }
    }

}
