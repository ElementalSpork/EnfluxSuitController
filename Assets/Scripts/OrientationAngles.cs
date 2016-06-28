using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class OrientationAngles : MonoBehaviour {

    private Queue<float[]> angles = new Queue<float[]>();
    private bool show = false;
    private bool co = false;

    void Update()
    {
        if (show)
        {
            if (!co)
            {
                StartCoroutine(showAngles());
            }
        }
    }

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
    }

    public void stopShowing()
    {
        show = false;
    }

    private IEnumerator showAngles()
    {
        co = true;
       
            lock (angles)
            {
                getAngles();
            }

            Debug.Log("HEY");
            
            yield return null;
       

        co = false;
    }
	
}
