using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AudioProducer : MonoBehaviour
{
    static List<AudioPerception> _audioPercievers;
    private void Awake()
    {
        //Gets all the audio perceivers in the scene
        _audioPercievers = FindObjectsOfType<AudioPerception>().ToList();
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
        //Cycles through each perciever in the percievers list
        foreach (AudioPerception perciever in _audioPercievers)
        {
            //Gets the distance between this producer and the current perciever
            float dist = Vector3.Distance(origin, perciever.transform.position);

            //Optimization check.
            if (dist > maxDistance) continue;

            //if it is within a certain distance guarantee this sound to be made. This allows for walls to have a small amount of sound leakage
            else if(dist < 1.5f)
            {
                perciever.AddSound(origin, val / 3.0f);
                continue;
            }

            //Creates a nav mesh path and sees if a path is possible between the sound origin and the perciever.
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(origin, perciever.transform.position, ~0, path))
            {
                //Creates a total distance 
                float totalDistance = 0.0f;

                //Checks if the distance between the origin and the perciever is within the range
                if (InRange(origin, path, maxDistance, ref totalDistance))
                {
                    //Calculates a percentage of the sound heard based on distance traveled
                    float percentageOfMaxDistance = totalDistance / maxDistance;
                    float heard = val * (1 - percentageOfMaxDistance);
                    perciever.AddSound(origin, heard);
                }
            }
        }
    }

    public static AudioPerception GetClosestPerciever(Vector3 origin, float maxDistance)
    {
        //Holds the closest perciever and the distance
        AudioPerception closestPerciever = null;
        float closestDistance = float.MaxValue;

        foreach (AudioPerception perciever in _audioPercievers)
        {
            //Optimization check.
            float dist = Vector3.Distance(origin, perciever.transform.position);
            if (dist > maxDistance) continue;

            //Checks if a path is possible
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(origin, perciever.transform.position, ~0, path))
            {
                //Gets the distance between the sound and the perciever
                float totalDistance = 0.0f;
                if (InRange(origin, path, maxDistance, ref totalDistance))
                {
                    //If the total distance is less than the current closest distance
                    if(totalDistance < closestDistance)
                    {
                        //Then set the closest values
                        closestPerciever = perciever;
                        closestDistance = totalDistance;
                    }
                }
            }
        }

        return closestPerciever;
    }
    static bool InRange(Vector3 origin, NavMeshPath path, float maxDistance, ref float distance)
    {
        //Sets the current position to the origin point
        Vector3 position = origin;
        //Cycles through each point in the path
        foreach (Vector3 corner in path.corners)
        {
            //Adds the distance based on a euclidian distance check
            distance += Vector3.Distance(position, corner);
            //Sets the new current position to this corner
            position = corner;

            //If the total distance is greater than the max distance then this perciever is not in range
            if (distance >= maxDistance) return false;
        }

        //Perciever is in range
        return true;
    }
}
