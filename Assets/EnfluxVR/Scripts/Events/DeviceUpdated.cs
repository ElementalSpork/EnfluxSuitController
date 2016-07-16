//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Device updated Event. This would only be used during scan to
//          update RSSI
//
//======================================================================

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
