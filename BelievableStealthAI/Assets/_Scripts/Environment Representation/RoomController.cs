using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<ObservableObject> ObservablesInRoom { get => _observables; }
    public List<Container> ContainersInRoom { get => _containers; }
    public List<AIAgent> AgentsInRoom { get => _aiInRoom; }


    [SerializeField] List<ObservableObject> _observables;
    [SerializeField] List<Container> _containers;
    [SerializeField] List<AIAgent> _aiInRoom;

    [ExecuteInEditMode]
    public void PerformAllRooms()
    {
        List<RoomController> rooms = FindObjectsOfType<RoomController>().ToList();
        foreach(RoomController room in rooms)
        {
            room.CreateRoomObjects();
        }
    }

    [ExecuteInEditMode]
    public void CreateRoomObjects()
    {
        _observables = new List<ObservableObject>();
        _containers = new List<Container>();
        _aiInRoom = new List<AIAgent>();

        List<ObservableObject> unfilteredObservables = FindObjectsOfType<ObservableObject>().ToList();
        List<Container> unfilteredContainer = FindObjectsOfType<Container>().ToList();
        List<AIAgent> unfilteredAgents = FindObjectsOfType<AIAgent>().ToList();

        Debug.Log(unfilteredObservables.Count + " observables");
        Debug.Log(unfilteredContainer.Count + " containers");
        Debug.Log(unfilteredAgents.Count + " agents");

        foreach (ObservableObject obv in unfilteredObservables)
        {
            Vector3 direction = (obv.transform.position - transform.position);

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 35.0f, ~0, QueryTriggerInteraction.Ignore))
            {
                ObservableObject o = hit.transform.GetComponentInParent<ObservableObject>();
                if (o)
                {
                    Debug.Log("Hitting: " + hit.transform.name);
                    if (!_observables.Contains(o))
                        _observables.Add(o);
                    continue;
                }
                o = hit.transform.GetComponentInChildren<ObservableObject>();
                if (o)
                {
                    Debug.Log("Hitting: " + hit.transform.name);
                    if (!_observables.Contains(o))
                        _observables.Add(o);
                    continue;
                }
            }
        }

        foreach (Container obv in unfilteredContainer)
        {
            Vector3 direction = (obv.transform.position - transform.position);

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 35.0f, ~0, QueryTriggerInteraction.Ignore))
            {
                Container o = hit.transform.GetComponentInParent<Container>();
                if (o)
                {
                    Debug.Log("Hitting: " + hit.transform.name);
                    if (!_containers.Contains(o))
                        _containers.Add(o);
                    continue;
                }
                o = hit.transform.GetComponentInChildren<Container>();
                if (o)
                {
                    Debug.Log("Hitting: " + hit.transform.name);
                    if (!_containers.Contains(o))
                        _containers.Add(o);
                    continue;
                }
            }
        }

        foreach (AIAgent obv in unfilteredAgents)
        {
            Vector3 direction = (obv.transform.position - transform.position);

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 35.0f, ~0, QueryTriggerInteraction.Ignore))
            {
                AIAgent o = hit.transform.GetComponentInParent<AIAgent>();
                if (o)
                {
                    Debug.Log("Hitting: " + hit.transform.name);
                    if (!_aiInRoom.Contains(o))
                        _aiInRoom.Add(o);
                    continue;
                }
                o = hit.transform.GetComponentInChildren<AIAgent>();
                if (o)
                {
                    Debug.Log("Hitting: " + hit.transform.name);
                    if (!_aiInRoom.Contains(o))
                        _aiInRoom.Add(o);
                    continue;
                }
            }
        }
    }
        
}
