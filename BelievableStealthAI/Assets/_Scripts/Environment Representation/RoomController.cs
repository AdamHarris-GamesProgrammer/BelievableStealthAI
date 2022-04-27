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
        //Sets the current room for each AI to this room
        _aiInRoom.ForEach(agent => agent.CurrentRoom = this);
    }

    public void AlertAllEnemies()
    {
        //Force alerts all enemies in the room without playing dialogue
        _aiInRoom.ForEach(agent => agent.ForceAlert(false));
    }

    public bool OutOfLookPoints()
    {
        //If the current look point is greater than the count
        if (_currentLP >= _lookAroundPoints.Count)
        {
            //Loop through each agent and tell them to stop searching
            //_aiInRoom.ForEach(agent => agent.StopSearching = true);
            foreach (AIAgent agent in _aiInRoom)
            {
                agent.StopSearching = true;
            }

            //Reset the look point to index 0
            _currentLP = 0;

            //Returns true (we are out of look point)
            return true;
        }

        //Returns false (we have more look points)
        return false;
    }

    public bool GetNextLookPoint(ref Transform lp)
    {
        //If the current look point is greater than the amount of look points
        if (_currentLP >= _lookAroundPoints.Count)
        {
            return false;
        }

        //Set the passed in look point to the one for this index
        lp = _lookAroundPoints[_currentLP];
        _currentLP++;

        //Return true
        return true;
    }

    public bool OutOfPOIs()
    {
        //If the current poi index is greater than the amount of poi 
        if (_currentPOI >= _pois.Count) {
            //Stop the ais in this room from searching
            //_aiInRoom.ForEach(agent => agent.StopSearching = true);
            foreach (AIAgent agent in  _aiInRoom)
            {
                agent.StopSearching = true;
            }

            //Resets the current poi index to 0
            _currentPOI = 0;

            Debug.Log("OUT OF POI");

            //Returns true (we are out of POI)
            return true;
        }
        Debug.Log("NOT OUT OF POI");
        //Returns false (we still have POI)
        return false;
    }

    public bool GetNextPOI(ref PointOfInterest poi)
    {
        //If we are out of POI
        if (_currentPOI >= _pois.Count)
        {
            return false;
        }

        //Set the passed in POI to the current one
        poi = _pois[_currentPOI];
        _currentPOI++;

        return true;
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    public void PerformAllRooms()
    {
        //Cycle through every room controller in the scene and find the rooms objects
        foreach(RoomController room in FindObjectsOfType<RoomController>())
        {
            EditorUtility.SetDirty(room);
            room.CreateRoomObjects();
        }
    }

    [ExecuteInEditMode]
    public void ClearRoom()
    {
        //resets all of the lists
        _observables = new List<ObservableObject>();
        _pois = new List<PointOfInterest>();
        _aiInRoom = new List<AIAgent>();
        _lookAroundPoints = new List<Transform>();
    }

    [ExecuteInEditMode]
    public void ClearAllRooms()
    {
        //Cycles through each room controller in the scene and clears them
        foreach (RoomController room in FindObjectsOfType<RoomController>())
        {
            EditorUtility.SetDirty(room);
            room.ClearRoom();
        }
    }


    [ExecuteInEditMode]
    public void CreateRoomObjects()
    {
        //Creates each list
        _observables = new List<ObservableObject>();
        _pois = new List<PointOfInterest>();
        _aiInRoom = new List<AIAgent>();
        _lookAroundPoints = new List<Transform>();
        _lightswitches = new List<Lightswitch>();

        //Get the unfiltered objects
        List<ObservableObject> unfilteredObservables = FindObjectsOfType<ObservableObject>().ToList();
        List<Container> unfilteredContainer = FindObjectsOfType<Container>().ToList();
        List<AIAgent> unfilteredAgents = FindObjectsOfType<AIAgent>().ToList();
        List<Lightswitch> unfilteredLightswitches = FindObjectsOfType<Lightswitch>().ToList();

        //Gets all transform components in children but skips the transform component on this object
        _lookAroundPoints = GetComponentsInChildren<Transform>(true)
            .Where(x => x.gameObject.transform.parent != transform.parent).ToList();

        foreach (ObservableObject obv in unfilteredObservables)
        {
            //Finds the direction between this observable and the room controller
            Vector3 direction = (obv.transform.position - transform.position);

            //Checks for raycast
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 35.0f, ~0, QueryTriggerInteraction.Ignore))
            {
                //Gets the observable object attached to the hit
                ObservableObject o = hit.transform.GetComponentInParent<ObservableObject>();
                if (o)
                {
                    //Adds the observable to the list if possible
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

        //Same pattern as above
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
        //Same pattern as above
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
        //Same pattern as above
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
