using UnityEngine;
using System.Collections;
using System;

public class EVRLowerLimbMap : EVRHumanoidLimbMap, ILimbAnimator {

    private Quaternion chain;
    private float[] initWaist = new float[] { 0, 0, 0, 0 };
    private float[] initLeftThigh = new float[] { 0, 0, 0, 0 };
    private float[] initLeftShin = new float[] { 0, 0, 0, 0 };
    private float[] initRightThigh = new float[] { 0, 0, 0, 0 };
    private float[] initRightShin = new float[] { 0, 0, 0, 0 };


    // Use this for initialization
    void Start () {
	
	}

    public void setInit()
    {
        initState = InitState.INIT;
    }

    public void resetInit()
    {
        initState = InitState.PREINIT;
    }

    public void operate(float[] angles)
    {
        if(angles != null)
        {
            if (initState == InitState.PREINIT && angles != null)
            {
                Buffer.BlockCopy(angles, 0, initWaist, 0, 4 * sizeof(float));
                Buffer.BlockCopy(angles, 4 * sizeof(float), initLeftThigh, 0, 4 * sizeof(float));
                Buffer.BlockCopy(angles, 8 * sizeof(float), initLeftShin, 0, 4 * sizeof(float));
                Buffer.BlockCopy(angles, 12 * sizeof(float), initRightThigh, 0, 4 * sizeof(float));
                Buffer.BlockCopy(angles, 16 * sizeof(float), initRightShin, 0, 4 * sizeof(float));
            }
            else if (initState == InitState.INIT)
            {
                if (getLiveHMD())
                {
                    waist.localRotation = Quaternion.Inverse(hmd.localRotation);
                }

                //PANTS NOT INTEGRATED YET
            }
        }
    }
}
