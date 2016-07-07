using UnityEngine;
using System.Collections;
using System;

public class EVRHumanoidLimbMap : MonoBehaviour, EVRSuitManager.IAddOrientationAngles
{
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
    public int _value = 6;

    private enum AnimState
    {
        UNANIMATED,
        ANIMATING_UPPER,
        ANIMATING_LOWER,
        ANIMATING_FULL
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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void stopRealTime()
    {
        animState = AnimState.UNANIMATED;
    }

    private IEnumerator anglesUpdater()
    {
        while(animState != AnimState.UNANIMATED)
        {
            float[] updated = updateOrientations.getAngles();
            animator.operate();
            yield return null;
        }
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
                Debug.Log("Set mode upper");
                break;
            case 2:
                //instantiate
                animState = AnimState.ANIMATING_LOWER;
                break;
            case 3:
                animState = AnimState.ANIMATING_FULL;
                break;
            default:
                animState = AnimState.UNANIMATED;
                Debug.Log("Error, unable to set mode");
                break;
        }

        StartCoroutine(anglesUpdater());
    }
}
