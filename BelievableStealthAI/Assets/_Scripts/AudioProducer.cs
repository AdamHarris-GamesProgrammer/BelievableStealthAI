using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AudioProducer : MonoBehaviour
{
    static List<AudioPerception> _audioPercievers;

    static void Init()
    {
        _audioPercievers = FindObjectsOfType<AudioPerception>().ToList();
    }

    private void Awake()
    {
        Init();
    }

    public static void AddPerciever(AudioPerception perciever)
    {
        _audioPercievers.Add(perciever);
    }

    public static void RemovePerciever(AudioPerception perciever)
    {
        _audioPercievers.Remove(perciever);
    }

    public static void ProduceSound(Vector3 origin, float val, float maxDistance)
    {

        foreach (AudioPerception perciever in _audioPercievers)
        {
            float totalDistance = 0.0f;

            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(origin, perciever.transform.position, ~0, path))
            {
                if (InRange(origin, path, maxDistance, ref totalDistance))
                {
                    float percentageOfMaxDistance = totalDistance / maxDistance;
                    float heard = val * (1 - percentageOfMaxDistance);
                    perciever.AddSound(origin, heard);
                }
            }
        }
    }
    static bool InRange(Vector3 origin, NavMeshPath path, float maxDistance, ref float distance)
    {
        
        Vector3 position = origin;
        foreach (Vector3 corner in path.corners)
        {
            distance += Vector3.Distance(position, corner);
            position = corner;

            if (distance >= maxDistance) return false;
        }

        return true;
    }
}
