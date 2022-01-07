using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Ragdoll : MonoBehaviour
{
    Rigidbody[] _rigidbodies;
    Animator _animator;

    float _lifeTime = 5.0f;
    float _timer = 0.0f;
    bool _active = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        //Gets all the rigidbodies in children
        var bodies = new HashSet<Rigidbody>(GetComponentsInChildren<Rigidbody>());

        bodies.Remove(GetComponent<Rigidbody>());

        //Converts to an array
        _rigidbodies = bodies.ToArray();
        //Deativates the ragdoll
        DeactivateRagdoll();
    }

    private void Update()
    {
        if (_active)
        {
            //Adds time to the timer
            _timer += Time.deltaTime;

            //Checks the lifetime
            if(_timer > _lifeTime)
            {
                //Cycles through each rigidbody and destroys the character joint and the rigidbody (CharacterJoint depends on rigidbody) 
                foreach(var rb in _rigidbodies)
                {
                    if (rb == null) continue;
                    Destroy(rb.GetComponent<CharacterJoint>());
                    Destroy(rb);
                }
                
                //Destroys this component afterwards.
                Destroy(this);
            }
        }
    }

    public void DeactivateRagdoll()
    {
        _animator.enabled = true;
        //Makes all the rigidbodies kinematic, stopping them from reacting to gravity
        System.Array.ForEach(_rigidbodies, rb => rb.isKinematic = true);
    }

    public void ActivateRagdoll()
    {
        if (_active) return;
        _active = true;
        //Disables the animator and lets physics take over
        _animator.enabled = false;
        foreach (var rb in _rigidbodies)
        {
            if (rb == null) continue;

            if (rb.gameObject.CompareTag("Enemy")) continue;
            //The rigidbody can now react to physics
            rb.isKinematic = false;
            Vector3 newVel = rb.velocity;
            newVel.z = 0.0f;
            rb.velocity = newVel;
        }
    }
}
