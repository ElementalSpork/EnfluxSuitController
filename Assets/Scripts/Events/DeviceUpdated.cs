using UnityEngine;
using System.Collections;

public class DeviceUpdated {

    public delegate void UpdateAction();
    public static event UpdateAction OnUpdated;

    public void Updated(string update)
    {     
        if (OnUpdated != null)
        {
            OnUpdated();            
        }
    }
}
