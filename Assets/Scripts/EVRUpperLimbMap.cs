using UnityEngine;
using System;

public class EVRUpperLimbMap : EVRHumanoidLimbMap, ILimbAnimator {
    JointRotations jointRotations = new JointRotations();
    private Quaternion chain;

    private float[] initCore = new float[] { 0, 0, 0, 0 };
    private float[] initLeftUpper = new float[] { 0, 0, 0, 0 };
    private float[] initLeftFore = new float[] { 0, 0, 0, 0 };
    private float[] initRightUpper = new float[] { 0, 0, 0, 0 };
    private float[] initRightFore = new float[] { 0, 0, 0, 0 };

    public void setInit()
    {
        initState = InitState.INIT;
    }

    public void resetInit()
    {
        initState = InitState.PREINIT;
    }

    //interface method
    public void operate(float[] angles)
    {
        //parse angles
        //apply to upper        
        if (initState == InitState.PREINIT && angles != null)
        {
            //do initialization            
            Buffer.BlockCopy(angles, 0, initCore, 0, 4 * sizeof(float));
            Buffer.BlockCopy(angles, 4 * sizeof(float), initLeftUpper, 0, 4 * sizeof(float));
            Buffer.BlockCopy(angles, 8 * sizeof(float), initLeftFore, 0, 4 * sizeof(float));
            Buffer.BlockCopy(angles, 12 * sizeof(float), initRightUpper, 0, 4 * sizeof(float));
            Buffer.BlockCopy(angles, 16 * sizeof(float), initRightFore, 0, 4 * sizeof(float));

        } else if (initState == InitState.INIT && angles != null)
        {
            //do main animating            
            //core.localRotation = Quaternion.AngleAxis(angles[3] - initCore[3], Vector3.up) *
            //    Quaternion.AngleAxis(angles[2] - initCore[2], Vector3.left) *
            //    Quaternion.AngleAxis(angles[1] - initCore[1], Vector3.back);

            //Left Upper user node 2
            //90 deg transform puts snesor in correct orientation
            chain = Quaternion.AngleAxis(angles[7], Vector3.up) *
                Quaternion.AngleAxis(angles[6], Vector3.left) *
                Quaternion.AngleAxis(angles[5], Vector3.back) *
                Quaternion.AngleAxis(270, Vector3.down);
            leftUpper.localRotation = Quaternion.Inverse(core.localRotation) * chain;

            //Left Fore node 4    
            chain = Quaternion.AngleAxis(angles[11], Vector3.up) *
                Quaternion.AngleAxis(angles[10], Vector3.left) *
                Quaternion.AngleAxis(angles[9], Vector3.back) *
                Quaternion.AngleAxis(270, Vector3.down);
            leftFore.localRotation = Quaternion.Inverse(leftUpper.localRotation) *
                Quaternion.Inverse(core.localRotation) * chain;

            ////Right Upper node 3
            //chain = Quaternion.AngleAxis(angles[15], Vector3.up) *
            //    Quaternion.AngleAxis(angles[14], Vector3.left) *
            //    Quaternion.AngleAxis(angles[13], Vector3.back) *
            //    Quaternion.AngleAxis(270, Vector3.up);
            //rightUpper.localRotation = Quaternion.Inverse(core.localRotation) * chain;

            ////Right Fore (Animation) Right Fore (User) node 5
            //chain = Quaternion.AngleAxis(angles[19], Vector3.up) *
            //    Quaternion.AngleAxis(angles[18], Vector3.left) *
            //    Quaternion.AngleAxis(angles[17], Vector3.back) *
            //    Quaternion.AngleAxis(270, Vector3.up);
            //rightFore.localRotation = Quaternion.Inverse(rightUpper.localRotation) *
            //    Quaternion.Inverse(core.localRotation) * chain;
        }
    }
}