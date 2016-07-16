//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Buffer for incoming data from EnfluxVR suit
//
//======================================================================

using UnityEngine;
using System.Collections.Generic;

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
        if (angles.Count > 0)
        {
            return angles.Dequeue();
        }else
        {
            return null;
        }
    }
}