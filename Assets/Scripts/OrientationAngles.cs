using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class OrientationAngles : MonoBehaviour, EVRSuitManager.IAddOrientationAngles,
    EVRHumanoidLimbMap.IGetOrientationAngles{

    private Queue<float[]> angles = new Queue<float[]>();
    private bool show = false;
    private System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
    private string mode;

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

    public void setMode(string mode)
    {
        this.mode = mode;
    }

    public string getMode()
    {
        return mode;
    }

    //public void getAngles()
    //{
    //    //StringBuilder result = new StringBuilder();
    //    /*foreach (float a in angles.Dequeue()){
    //        result.Append(a);
    //        result.Append(",");
    //    }*/

    //    //Debug.Log(result.ToString());
    //    Debug.Log(timer.Elapsed);       
    //}

    //public void startShowingAngles()
    //{
    //    show = true;
    //    timer.Start();
    //    StartCoroutine(showAngles());
    //}

    //public void stopShowing()
    //{
    //    show = false;
    //}    

    //private IEnumerator showAngles()
    //{
    //    while (show)
    //    {
    //        getAngles();
    //        yield return null;
    //    }
    //}
}