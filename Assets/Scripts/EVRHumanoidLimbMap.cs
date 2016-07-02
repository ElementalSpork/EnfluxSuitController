using UnityEngine;
using System.Collections;

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
    private AnimState animState = AnimState.UNANIMATED;

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
        updateOrientations = GameObject.Find("OrientationAngles").GetComponent<IGetOrientationAngles>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void realTimeAnimate()
    {
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
