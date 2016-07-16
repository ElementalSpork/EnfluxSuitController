//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Mapping and operations of EnfluxVR demo humanoid
//
//======================================================================

using UnityEngine;
using System.Collections;

public class EVRHumanoidLimbMap : MonoBehaviour, EVRSuitManager.IAddOrientationAngles
{
    public Transform hmd;
    public Transform head;
    public Transform root;
    public Transform cameraAnchor;
    //lower    
    public Transform waist;
    public Transform leftThigh;
    public Transform leftShin;
    public Transform rightThigh;
    public Transform rightShin;
    //upper
    public Transform core;
    public Transform leftUpper;
    public Transform leftFore;
    public Transform rightUpper;
    public Transform rightFore;

    private OrientationAngles updateOrientations;
    private ILimbAnimator animator;
    private AnimState animState = AnimState.UNANIMATED;
    public InitState initState = InitState.PREINIT;
    private string requestMode;
    private bool liveHMD = true;

    private enum AnimState
    {
        UNANIMATED,
        ANIMATING_UPPER,
        ANIMATING_LOWER,
        ANIMATING_FULL
    };

    public enum InitState
    {
        PREINIT,
        INIT
    };

    public interface IGetOrientationAngles
    {
        void addAngles(float[] latest);
        float[] getAngles();
    }

    // Use this for initialization
    void Start () {
        updateOrientations = GameObject.Find("OrientationAngles")
            .GetComponent<OrientationAngles>();

        if (hmd.name == "HMD_PLACEHOLDER")
        {
            liveHMD = false;
        }
    }

    private IEnumerator anglesUpdater()
    {
        while(animState != AnimState.UNANIMATED)
        {
            float[] updated = updateOrientations.getAngles();
            animator.operate(updated);
            yield return null;
        }
    }

    public bool getLiveHMD()
    {
        return liveHMD;
    }

    public void initialize()
    {
        animator.setInit();
    }   

    public void resetInitialize()
    {
        animator.resetInit();
    }

    //interface method
    public string getMode()
    {
        return requestMode;
    }

    //interface method
    public void addAngles(float[] angles)
    {        
        updateOrientations.addAngles(angles);        
    }

    //interface method
    public void setMode(int mode)
    {
        Debug.Log(mode);
        switch (mode)
        {
            case 0:                            
                //set state
                animState = AnimState.UNANIMATED;
                break;
            case 1:                
                animator = GameObject.Find("EVRUpperLimbMap")
                    .GetComponent<EVRUpperLimbMap>();
                animState = AnimState.ANIMATING_UPPER;
                requestMode = "requestup";
                Debug.Log("Set mode upper");
                break;
            case 2:
                //instantiate
                animator = GameObject.Find("EVRLowerLimbMap")
                    .GetComponent<EVRLowerLimbMap>();
                animState = AnimState.ANIMATING_LOWER;
                requestMode = "requestlow";
                Debug.Log("Set mode lower");
                break;
            case 3:
                animState = AnimState.ANIMATING_FULL;
                animator = GameObject.Find("EVRFullAnimator").GetComponent<EVRFullAnimator>();
                requestMode = "request";
                break;
            default:
                animState = AnimState.UNANIMATED;
                Debug.Log("Error, unable to set mode");
                break;
        }

        StartCoroutine(anglesUpdater());
    }
}
