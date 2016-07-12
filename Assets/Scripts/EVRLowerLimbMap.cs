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
                waist.localRotation = Quaternion.AngleAxis(angles[3] - initWaist[3], Vector3.up) *
                    Quaternion.AngleAxis(angles[2] - initWaist[2], Vector3.left) *
                    Quaternion.AngleAxis(angles[1] - initWaist[1], Vector3.back);

                //node 8, left thigh user, right thigh animation
                chain = Quaternion.AngleAxis(angles[7], Vector3.up) *
                    Quaternion.AngleAxis(angles[6] - initLeftThigh[2], Vector3.left) *
                    Quaternion.AngleAxis(angles[5] - initLeftThigh[1], Vector3.back);
                leftThigh.localRotation = Quaternion.Inverse(waist.localRotation) * chain;

                //node 10, left shin user, right shin animation
                chain = Quaternion.AngleAxis(angles[11], Vector3.up) *
                    Quaternion.AngleAxis(angles[10] - initLeftShin[2], Vector3.left) *
                    Quaternion.AngleAxis(angles[9] - initLeftShin[1], Vector3.back);
                leftShin.localRotation = Quaternion.Inverse(leftThigh.localRotation) *
                    Quaternion.Inverse(waist.localRotation) * chain;

                ////node 9, right thigh user, left thigh animation
                chain = Quaternion.AngleAxis(angles[15], Vector3.up) *
                    Quaternion.AngleAxis(angles[14] - initRightThigh[2], Vector3.left) *
                    Quaternion.AngleAxis(angles[13] - initRightThigh[1], Vector3.back);
                rightThigh.localRotation = Quaternion.Inverse(waist.localRotation) * chain;

                //node 11, right shin user, left shin animation
                chain = Quaternion.AngleAxis(angles[19], Vector3.up) *
                    Quaternion.AngleAxis(angles[18] - initRightShin[2], Vector3.left) *
                    Quaternion.AngleAxis(angles[17] - initRightShin[1], Vector3.back);
                rightShin.localRotation = Quaternion.Inverse(rightThigh.localRotation) *
                    Quaternion.Inverse(waist.localRotation) * chain;
                    
            }
        }
    }
}
