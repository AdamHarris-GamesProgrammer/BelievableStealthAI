using UnityEngine;
using UnityEngine.AI;
using System.Collections;


[RequireComponent(typeof(NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
    AIAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<AIAgent>();
    }

    IEnumerator Start()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;

        //Eternally loops
        while (true)
        {
            //If the agent is currently on a offmeshlink 
            if (agent.isOnOffMeshLink)
            {
                //Starts the movement coroutine 
                yield return StartCoroutine(NormalSpeed(agent));

                //Completes the off mesh link
                agent.CompleteOffMeshLink();
            }
            yield return null;
        }
    }

    IEnumerator NormalSpeed(NavMeshAgent agent)
    {
        OffMeshLinkData data = agent.currentOffMeshLinkData;

        //Opens the nearby observable (door or window) if available
        if(_agent.NearbyObservable)
        {
            _agent.NearbyObservable.Open();
        }

        //Calculates an end position based on end position of the off mesh link and the agents offset
        Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

        //Loops while the agents position is not the same as the end position
        while (agent.transform.position != endPos)
        {
            //Calculates a movement speed based on half the agents speed
            float movementSpeed = (agent.speed / 2.0f) * Time.deltaTime;
            //Applies the movement speed to the animation
            agent.GetComponent<Animator>().SetFloat("movementSpeed", movementSpeed);

            //Sets the agents position based on moving towards the end pos
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, movementSpeed);
            yield return null;
        }
    }
}