using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OrientationAngles : MonoBehaviour, EVRHumanoidLimbMap.IGetOrientationAngles{

    private Queue<float[]> angles = new Queue<float[]>();
    private bool show = false;
    //private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();    

    public void addAngles(float[] latest)
    {
        angles.Enqueue(latest);
    }

    public float[] getAngles()
    {
        if(angles.Count > 0)
        {
            return angles.Dequeue();
        }else
        {
            return null;
        }
    }
}