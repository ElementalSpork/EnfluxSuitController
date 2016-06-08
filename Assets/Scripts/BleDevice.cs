using UnityEngine;
using System.Collections;

public class BleDevice {

    private string _name;
    public string name
    {   set { _name = value;}
        get { return _name;}
    }
    private string _rssi;
    public string rssi
    {   set { _rssi = value; }
        get { return _rssi;  }
    }
    private string _mac;
    public string mac
    {   set { _mac = value;}
        get { return _mac; }
    }

    public BleDevice(string mac, string rssi, string name)
    {
        _mac = mac;        
        _rssi = rssi;
        _name = name;
    }

    public override string ToString()
    {
        return "  " + _mac + "    " + _name + "    " + _rssi;
    }
}
