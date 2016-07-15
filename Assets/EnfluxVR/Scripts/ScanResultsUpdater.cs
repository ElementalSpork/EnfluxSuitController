using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EnflxStructs;

public class ScanResultsUpdater : MonoBehaviour {

    private state status;
    private IScanUpdate updateView;
    private Dictionary<string, BleDevice> scannedDevices = new Dictionary<string, BleDevice>();
        
    private enum state
    {
        state_updating,
        state_notupdating
    };

    // Use this for initialization
    void Start () {
        status = state.state_notupdating;
	
	}
	
	// Update is called once per frame
	void Update () {
        if(status == state.state_notupdating)
        {
            scannedDevices = ThreadDispatch.instance.GetScanItems();
            if(scannedDevices != null)
            {
                StartCoroutine(processDevices());
                status = state.state_updating;
            }
        }
    }

    private IEnumerator processDevices()
    {
        foreach(var pair in scannedDevices)
        {
            updateView.postUpdate(pair.Value);
            yield return null;
        }

        status = state.state_notupdating;
    }

    public void setUpdateView(IScanUpdate view)
    {
        updateView = view;
    }

    public interface IScanUpdate
    {
        void postUpdate(BleDevice device);
    }
}
