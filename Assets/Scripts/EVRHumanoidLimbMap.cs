using UnityEngine;
using System.Collections;
using System;

public class EVRHumanoidLimbMap : MonoBehaviour {
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

    private IGetOrientationAngles updateOrientations;
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
        float[] getAngles();
        string getMode();
    }

    // Use this for initialization
    void Start () {
        updateOrientations = GameObject.Find("OrientationAngles")
            .GetComponent<IGetOrientationAngles>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //todo: testing method, remove this
    public void InstantiateTester()
    {
        animator = GameObject.Find("EVRUpperLimbMap").GetComponent<EVRUpperLimbMap>();
        //animator = gameObject.AddComponent<EVRUpperLimbMap>();
        animState = AnimState.ANIMATING_UPPER;        
        StartCoroutine(testRotations());
    }

    //todo: testing method, remove this
    public void StopInstantiateTester()
    {
        animState = AnimState.UNANIMATED;
    }

    //todo: testing method, remove this
    private IEnumerator testRotations()
    {
        while (animState == AnimState.ANIMATING_UPPER)
        {
            animator.operate();
            yield return null;
        }
    }

    public void realTimeAnimate()
    {
        string mode = updateOrientations.getMode();
        switch (mode)
        {
            case "upper":
                //instantiate                
                //set state
                animState = AnimState.ANIMATING_UPPER;
                break;
            case "lower":
                //instantiate
                animState = AnimState.ANIMATING_LOWER;
                break;
            case "full":
                //instantiate
                animState = AnimState.ANIMATING_FULL;
                break;
            default:
                Debug.Log("Error, unable to set mode");
                break;
        }
        animState = AnimState.ANIMATING_FULL;
        StartCoroutine(anglesUpdater());
    }

    public void stopRealTime()
    {
        animState = AnimState.UNANIMATED;
    }

    private IEnumerator anglesUpdater()
    {
        while(animState == AnimState.ANIMATING_FULL)
        {
            float[] updated = updateOrientations.getAngles();
            Debug.Log(updated.Length);
            yield return null;
        }
    }
}
