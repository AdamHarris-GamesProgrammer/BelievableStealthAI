using UnityEngine;

public class DistractionObject : MonoBehaviour
{
    //Stops the rock from repeatedly distracting enemies
    bool _activated = false;

    //Radius for distracting agents
    [Min(0.01f)][SerializeField] float _distractionRadius = 15.0f;

    private void OnCollisionEnter(Collision collision)
    {
        //Stops this logic from being executed twice
        if (_activated) return;
        _activated = true;

        //Plays the sound effect
        GetComponent<AudioSource>().Play();

        //Gets the closest audio perciever (pathfind operation) within the distraction radius
        AudioPerception perc = AudioProducer.GetClosestPerciever(transform.position, _distractionRadius);

        if(perc != null)
        {
            //Sets the agent as distracted
            perc.GetComponentInParent<AIAgent>().Distracted(transform.position);
        }

        //Gets the rigidbody attached to this object and alters the velocity/angular velocity. Stops unnecessary bouncing
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb)
        {
            rb.angularVelocity /= 5.0f;
            rb.velocity /= 5.0f;
            Destroy(rb, 5.0f);
        }
        
        //Gets and plays the particle system attached to this object
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps)
        {
            ps.Play();
            Destroy(ps, 5.0f);
        }

        //Destroys the rigidbody, particle system and this script after T amount of seconds.
        Destroy(this, 15.0f);
    }
}
