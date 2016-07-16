using UnityEngine;
using System.Collections;

public class calibrationOnClick : MonoBehaviour {

    private EVRSuitManager _manager;

    // Use this for initialization
    void Start () {
        _manager = GameObject.Find("EVRSuitManager").GetComponent<EVRSuitManager>();
    }

    public void startClick()
    {
        _manager.calibrateDevices();
    }

    public void stopClick()
    {
        _manager.finishCalibration();
    }
}
