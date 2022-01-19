using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AudioProducer : MonoBehaviour
{
    List<AudioPerception> _audioPercievers;
    private void Awake()
    {
        _audioPercievers = FindObjectsOfType<AudioPerception>().ToList();
    }

    public void ProduceSound(float val, float maxDistance)
    {
        foreach (AudioPerception perciever in _audioPercievers)
        {
            float totalDistance = 0.0f;

            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, perciever.transform.position, ~0, path))
            {
                if (InRange(path, maxDistance, ref totalDistance))
                {
                    float percentageOfMaxDistance = totalDistance / maxDistance;
                    float heard = val * (1 - percentageOfMaxDistance);
                    perciever.AddSound(transform.position, heard);
                }
            }
        }
    }
    bool InRange(NavMeshPath path, float maxDistance, ref float distance)
    {
        Vector3 position = transform.position;
        foreach (Vector3 corner in path.corners)
        {
            distance += Vector3.Distance(position, corner);
            position = corner;

            if (distance >= maxDistance) return false;
        }

        return true;
    }
}
