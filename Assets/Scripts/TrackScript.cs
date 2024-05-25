using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TrackScript : MonoBehaviour
{

    private CheckPoint[] checkPoints;

    void Start()
    {
        checkPoints = this.GetComponentsInChildren<CheckPoint>();
    }

    public int CrossCheckPoint(int id, Transform transform)
    {
        if (checkPoints[id].transform == transform)
        {
            return (id + 1) % checkPoints.Length;
        }
        else
        {
            return id;
        }
    }

    public Transform GetCheckPointTransform(int id)
    {
        return checkPoints[id % checkPoints.Length].transform;

    }
    
}
