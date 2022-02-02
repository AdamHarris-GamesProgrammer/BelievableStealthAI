using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<ObservableObject> ObservablesInRoom { get => _observables; }
    public List<Container> ContainersInRoom { get => _containers; }
    public List<AIAgent> AgentsInRoom { get => _aiInRoom; }

    public List<Transform> LookAroundPoints { get => _lookAroundPoints; }


    [SerializeField] List<ObservableObject> _observables;
    [SerializeField] List<Container> _containers;
    [SerializeField] List<AIAgent> _aiInRoom;
    [SerializeField] List<Transform> _lookAroundPoints;

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
    public void ClearRoom()
    {
        _observables = new List<ObservableObject>();
        _containers = new List<Container>();
        _aiInRoom = new List<AIAgent>();
        _lookAroundPoints = new List<Transform>();
    }

    [ExecuteInEditMode]
    public void ClearAllRooms()
    {
        List<RoomController> rooms = FindObjectsOfType<RoomController>().ToList();
        foreach (RoomController room in rooms)
        {
            room.ClearRoom();
        }
    }


    [ExecuteInEditMode]
    public void CreateRoomObjects()
    {
        _observables = new List<ObservableObject>();
        _containers = new List<Container>();
        _aiInRoom = new List<AIAgent>();
        _lookAroundPoints = new List<Transform>();

        List<ObservableObject> unfilteredObservables = FindObjectsOfType<ObservableObject>().ToList();
        List<Container> unfilteredContainer = FindObjectsOfType<Container>().ToList();
        List<AIAgent> unfilteredAgents = FindObjectsOfType<AIAgent>().ToList();

        //Gets all transform components in children but skips the transform component on this object
        _lookAroundPoints = GetComponentsInChildren<Transform>(true)
            .Where(x => x.gameObject.transform.parent != transform.parent).ToList();

        foreach (ObservableObject obv in unfilteredObservables)
        {
            Vector3 direction = (obv.transform.position - transform.position);

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 35.0f, ~0, QueryTriggerInteraction.Ignore))
            {
                ObservableObject o = hit.transform.GetComponentInParent<ObservableObject>();
                if (o)
                {
                    if (!_observables.Contains(o))
                    {
                        _observables.Add(o);
                        o.DecideRoom(this);
                    }
                    continue;
                }
                o = hit.transform.GetComponentInChildren<ObservableObject>();
                if (o)
                {
                    if (!_observables.Contains(o))
                    {
                        _observables.Add(o);
                        o.DecideRoom(this);
                    }
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
                    if (!_containers.Contains(o))
                    {
                        _containers.Add(o);
                        o.Room = this;
                    }
                    continue;
                }
                o = hit.transform.GetComponentInChildren<Container>();
                if (o)
                {
                    if (!_containers.Contains(o))
                    {
                        _containers.Add(o);
                        o.Room = this;
                    }
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
                    if (!_aiInRoom.Contains(o))
                    {
                        _aiInRoom.Add(o);
                        o.CurrentRoom = this;
                    }
                    continue;
                }
                o = hit.transform.GetComponentInChildren<AIAgent>();
                if (o)
                {
                    if (!_aiInRoom.Contains(o))
                    {
                        _aiInRoom.Add(o);
                        o.CurrentRoom = this;
                    }
                    continue;
                }
            }
        }
    }
        
}
