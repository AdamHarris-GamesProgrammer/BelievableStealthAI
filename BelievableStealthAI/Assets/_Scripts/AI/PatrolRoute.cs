using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    Transform[] _patrolPoints;
    [SerializeField] bool _loopBack = false;

    

    bool _goingForward = true;

    private void Awake()
    {
        var transforms = new HashSet<Transform>(GetComponentsInChildren<Transform>());
        transforms.Remove(transform);
        _patrolPoints = transforms.ToArray();
    }

    public Vector3 GetNextIndex(ref int index)
    {
        Vector3 nextPos;

        if (_loopBack)
        {
            index = (index += 1) % _patrolPoints.Length;
            nextPos = _patrolPoints[index].position;
        }
        else
        {
            if(_goingForward)
            {
                index++;

                if (index == _patrolPoints.Length)
                {
                    _goingForward = false;
                    index = _patrolPoints.Length - 1;
                }
            }
            else
            {
                index--;

                if(index == -1)
                {
                    _goingForward = true;
                    index = 0;
                }
            }

            nextPos = _patrolPoints[index].position;


        }

        return nextPos;
    }

    private void OnDrawGizmos()
    {
        var transforms = new HashSet<Transform>(GetComponentsInChildren<Transform>());
        transforms.Remove(transform);
        Transform[] points = transforms.ToArray();

        if (points[0] != null)
        {
            Vector3 pos = points[0].position;

            if(_loopBack)
            {
                foreach (Transform transform in points)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(transform.position, 0.5f);


                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(pos, transform.position);

                    pos = transform.position;
                }

                Gizmos.DrawLine(pos, points[0].position);
            }
            else
            {
                foreach (Transform transform in points)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(transform.position, 0.5f);


                    Gizmos.color = Color.white;
                    Gizmos.DrawLine(pos, transform.position);

                    pos = transform.position;
                }
            }
        }
    }
}
