using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<ObservableObject> ObservablesInRoom { get => _observables; }
    public List<AIAgent> AgentsInRoom { get => _aiInRoom; }
    public List<Transform> LookAroundPoints { get => _lookAroundPoints; }
    public List<PointOfInterest> PointsOfInterest { get => _pois; }


    [SerializeField] List<ObservableObject> _observables;
    [SerializeField] List<PointOfInterest> _pois;
    [SerializeField] List<AIAgent> _aiInRoom;
    [SerializeField] List<Transform> _lookAroundPoints;
    [SerializeField] List<Lightswitch> _lightswitches;


    [SerializeField] int _currentLP = 0;
    [SerializeField] int _currentPOI = 0;
    [SerializeField] int _currentObservable = 0;
    [SerializeField] int _currentLightSwitch = 0;

    private void Awake()
    {
        foreach (AIAgent agent in _aiInRoom)
        {
            agent.CurrentRoom = this;
        }
    }

    public void AlertAllEnemies()
    {
        foreach(AIAgent agent in _aiInRoom)
        {
            agent.ForceAlert(false);
        }
    }

    public bool OutOfLookPoints()
    {
        if (_currentLP >= _lookAroundPoints.Count)
        {
            foreach (AIAgent agent in _aiInRoom)
            {
                agent.StopSearching = true;
            }
            _currentLP = 0;

            return true;
        }
        return false;
    }

    public bool GetNextLookPoint(ref Transform lp)
    {
        if(_currentLP >= _lookAroundPoints.Count)
        {
            return false;
        }

        lp = _lookAroundPoints[_currentLP];
        _currentLP++;

        return true;
    }

    public bool OutOfPOIs()
    {
        if (_currentPOI >= _pois.Count) {
            foreach (AIAgent agent in _aiInRoom)
            {
                agent.StopSearching = true;
            }
            _currentPOI = 0;

            return true;
        }

        return false;
    }

    public bool GetNextPOI(ref PointOfInterest poi)
    {
        if(_currentPOI >= _pois.Count)
        {
            return false;
        }

        poi = _pois[_currentPOI];
        _currentPOI++;

        return true;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    public void PerformAllRooms()
    {
        List<RoomController> rooms = FindObjectsOfType<RoomController>().ToList();
        foreach(RoomController room in rooms)
        {
            EditorUtility.SetDirty(room);
            room.CreateRoomObjects();
        }
    }

    [ExecuteInEditMode]
    public void ClearRoom()
    {
        _observables = new List<ObservableObject>();
        _pois = new List<PointOfInterest>();
        _aiInRoom = new List<AIAgent>();
        _lookAroundPoints = new List<Transform>();
    }

    [ExecuteInEditMode]
    public void ClearAllRooms()
    {
        List<RoomController> rooms = FindObjectsOfType<RoomController>().ToList();
        foreach (RoomController room in rooms)
        {
            EditorUtility.SetDirty(room);
            room.ClearRoom();
        }
    }


    [ExecuteInEditMode]
    public void CreateRoomObjects()
    {
        _observables = new List<ObservableObject>();
        _pois = new List<PointOfInterest>();
        _aiInRoom = new List<AIAgent>();
        _lookAroundPoints = new List<Transform>();
        _lightswitches = new List<Lightswitch>();

        List<ObservableObject> unfilteredObservables = FindObjectsOfType<ObservableObject>().ToList();
        List<Container> unfilteredContainer = FindObjectsOfType<Container>().ToList();
        List<AIAgent> unfilteredAgents = FindObjectsOfType<AIAgent>().ToList();
        List<Lightswitch> unfilteredLightswitches = FindObjectsOfType<Lightswitch>().ToList();

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
                        EditorUtility.SetDirty(o);
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
                        EditorUtility.SetDirty(o);
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
                PointOfInterest o = hit.transform.GetComponentInParent<PointOfInterest>();
                if (o)
                {
                    if (!_pois.Contains(o))
                    {
                        _pois.Add(o);
                        EditorUtility.SetDirty(o);
                        o.Room = this;
                    }
                    continue;
                }
                o = hit.transform.GetComponentInChildren<PointOfInterest>();
                if (o)
                {
                    if (!_pois.Contains(o))
                    {
                        _pois.Add(o);
                        EditorUtility.SetDirty(o);
                        o.Room = this;
                    }
                    continue;
                }
            }
        }

        foreach (AIAgent obv in unfilteredAgents)
        {
            Vector3 adjustedPos = obv.transform.position + (Vector3.up * 1.5f);
            Vector3 direction = (adjustedPos - transform.position);

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 35.0f, ~0, QueryTriggerInteraction.Ignore))
            {
                AIAgent o = hit.transform.GetComponentInParent<AIAgent>();
                if (o)
                {
                    if (!_aiInRoom.Contains(o))
                    {
                        _aiInRoom.Add(o);
                        EditorUtility.SetDirty(o);
                        o.CurrentRoom = this;
                        EditorUtility.ClearDirty(o);
                    }
                    continue;
                }
                o = hit.transform.GetComponentInChildren<AIAgent>();
                if (o)
                {
                    if (!_aiInRoom.Contains(o))
                    {
                        _aiInRoom.Add(o);
                        EditorUtility.SetDirty(o);
                        o.CurrentRoom = this;
                        EditorUtility.ClearDirty(o);
                    }
                    continue;
                }
            }
        }

        foreach (Lightswitch ls in unfilteredLightswitches)
        {
            Vector3 direction = (ls.transform.position - transform.position);

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 35.0f, ~0, QueryTriggerInteraction.Ignore))
            {
                Lightswitch o = hit.transform.GetComponentInParent<Lightswitch>();
                if (o)
                {
                    if (!_lightswitches.Contains(o))
                    {
                        _lightswitches.Add(o);
                        EditorUtility.SetDirty(o);
                        o.Room = this;
                    }
                    continue;
                }
                o = hit.transform.GetComponentInChildren<Lightswitch>();
                if (o)
                {
                    if (!_lightswitches.Contains(o))
                    {
                        _lightswitches.Add(o);
                        EditorUtility.SetDirty(o);
                        o.Room = this;
                    }
                    continue;
                }
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        var transforms = new HashSet<Transform>(GetComponentsInChildren<Transform>());
        transforms.Remove(transform);
        Transform[] lookPoints = transforms.ToArray();

        foreach (Transform point in lookPoints)
        {
            Vector3 startPos = point.position + Vector3.up;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startPos, 0.5f);
            Gizmos.color = Color.blue;
            Vector3 endPoint = startPos + (point.forward * 2.5f);
            Gizmos.DrawLine(startPos, endPoint);
        }
    }

#endif
}
