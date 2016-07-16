//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Display and update discovered EnfluxVR modules
//
//======================================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Container_Devices : MonoBehaviour, ScanResultsUpdater.IScanUpdate {

    private EVRSuitManager _manager;
    private GameObject deviceToggle;
    public GameObject deviceScroll;
    private float height;
    private float ratio;
    private float containerWidth;
    private float scrollHeight;
    private float x;
    private float y;
    private int count = 0;
    private RectTransform toggleTransform;
    private RectTransform containerTransform;
    private RectTransform scrollTransform;
    private Dictionary<string, GameObject> displayedDevices = new Dictionary<string, GameObject>();

	// Use this for initialization
	void Start () {

        _manager = GameObject.Find("EVRSuitManager").GetComponent<EVRSuitManager>();

        GameObject.Find("ScanResultsUpdater")
            .GetComponent<ScanResultsUpdater>().setUpdateView(this);

        //Instantiate prefab in order to get dimensions
        deviceToggle = Instantiate(Resources.Load("Prefabs/Toggle_Device")) as GameObject;
        deviceToggle.transform.SetParent(deviceScroll.transform, false);

        toggleTransform = deviceToggle.GetComponent<RectTransform>();
        containerTransform = deviceScroll.GetComponent<RectTransform>();

        containerWidth = containerTransform.rect.width;
        ratio = toggleTransform.rect.height / toggleTransform.rect.width;
        height = containerWidth * ratio;

        //disable b/c not used
        deviceToggle.SetActive(false);
	}

    private void addToggleDevice(BleDevice device)
    {
        //calculated height of scroll container
        count++;
        scrollHeight = (height + 1.5f) * count;

        containerTransform.offsetMin = new Vector2(4.0f, -scrollHeight);
        containerTransform.offsetMax = new Vector2(-5.0f, 0);

        //create new entry
        GameObject newToggle = Instantiate(Resources.Load("Prefabs/Toggle_Device")) as GameObject;  
        
        newToggle.name = device.mac;
        newToggle.transform.SetParent(deviceScroll.transform, false);

        displayedDevices.Add(device.mac, newToggle);

        RectTransform newRect = newToggle.GetComponent<RectTransform>();
        
        x = containerTransform.anchorMax.x;
        y = scrollHeight - 1.5f;

        newRect.offsetMax = new Vector2(x, y);

        x = containerTransform.anchorMin.x;
        y = scrollHeight - height;

        newRect.offsetMin = new Vector2(x, y);

        newToggle.GetComponentInChildren<Text>().text = device.ToString();
    }

    private void updateToggleDevice(BleDevice device)
    {
        displayedDevices[device.mac].GetComponentInChildren<Text>().text = device.ToString();
    }

    public void postUpdate(BleDevice device)
    {
        if (!displayedDevices.ContainsKey(device.mac))
        {
            addToggleDevice(device);
        }else
        {
            updateToggleDevice(device);
        }
    }

    public void connectSelected()
    {
        List<string> selected = new List<string>();
        foreach(var pair in displayedDevices)
        {
            if (pair.Value.GetComponentInChildren<Toggle>().isOn)
            {
                selected.Add(pair.Key);
                Debug.Log(pair.Key);
            }
        }

        _manager.connectEnflux(selected);
    }

    public void disconnectSelected()
    {
        _manager.disconnectEnflux();
    }
}
