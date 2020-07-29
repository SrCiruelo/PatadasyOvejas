
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WavepointManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] wavepoints;

    public Vector3[] getClosestPointInRadius(float radius)
    {
        throw new System.NotImplementedException();
    }

    public Vector3[] getPointsInRadius(float radius)
    {
        throw new System.NotImplementedException();
    }

    public Vector3 getClosesPoint(Vector3 position)
    {
        float min_sqr_dist = float.MaxValue;
        Vector3 wavepoint_pos = position;

        foreach(GameObject obj in wavepoints)
        {
            float dist = (obj.transform.position - position).sqrMagnitude;
            if(dist < min_sqr_dist)
            {
                wavepoint_pos = obj.transform.position;
            }
        }
        return wavepoint_pos;
    }

    public Vector3 getRandomWavepoint()
    {
        return wavepoints[Random.Range(0, wavepoints.Length)].transform.position;
    }

    /**
     * Returns the nth closest waypoint, being 'n' a random number from 0 to 5.
     *
     * <returns>
     * The position of one of the closest waypoints
     * </returns>
     */
    public Vector3 getRandomClosest (Vector3 position)
    {
        SortedDictionary<float, Vector3> distances = new SortedDictionary<float, Vector3>();

        foreach (GameObject obj in wavepoints)
        {
            Vector3 pos = obj.transform.position;
            var dist = (pos - position).sqrMagnitude;
            distances.Add (dist, pos);
        }

        /* Unity's random sucks */
        var rng = new System.Random ();

        return distances.ElementAt (rng.Next (4)).Value;
    }
}
