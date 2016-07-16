//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Demo adapter for use with SteamVR and HTC Vive
//
//======================================================================

using UnityEngine;

public class SteamVRAdapter : MonoBehaviour {

    public GameObject hmd;
    //TEMPORARY FIX
    public Transform waist;

	// Use this for initialization
	void Start () {
        if(hmd != null)
        {
            gameObject.transform.SetParent(hmd.transform);
            GameObject.Find("[EnfluxVRHumanoid]").transform.SetParent(gameObject.transform);
            gameObject.transform.position = hmd.transform.position;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (hmd != null)
        {
            waist.localRotation = Quaternion.Inverse(hmd.transform.localRotation);
        }
	}
}
