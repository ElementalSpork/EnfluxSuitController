﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class OrientationAngles : MonoBehaviour {

    private Queue<float[]> angles = new Queue<float[]>();
    private bool show = false;    

    public void addAngles(float[] latest)
    {
        lock (angles)
        {
            angles.Enqueue(latest);            
        }
    }

    public void getAngles()
    {
        StringBuilder result = new StringBuilder();
        foreach (float a in angles.Dequeue()){
            result.Append(a);
            result.Append(",");
        }

        Debug.Log(result.ToString());
    }

    public void startShowingAngles()
    {
        show = true;
        StartCoroutine(showAngles());
    }

    public void stopShowing()
    {
        show = false;
    }

    private IEnumerator showAngles()
    {
        while (show)
        {
            lock (angles)
            {
                getAngles();
            }
            yield return null;
        }
    }
}
