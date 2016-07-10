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
            Array.Copy(angles, 0, initCore, 0, 4);
            Array.Copy(angles, 4, initLeftUpper, 0, 4);
            Array.Copy(angles, 8, initLeftFore, 0, 4);
            Array.Copy(angles, 12, initRightUpper, 0, 4);
            Array.Copy(angles, 4, initRightFore, 0, 4);


        } else if (initState == InitState.INIT && angles != null)
        {
            //do main animating            
            core.localRotation = Quaternion.AngleAxis(angles[3] - initCore[3], Vector3.down) *
                Quaternion.AngleAxis(angles[2] - initCore[2], Vector3.left) *
                Quaternion.AngleAxis(angles[1] - initCore[1], Vector3.forward);

            //node 2, left upper user, right upper animation
            chain = Quaternion.AngleAxis(angles[7], Vector3.down) *
                Quaternion.AngleAxis(angles[6], Vector3.left) *
                Quaternion.AngleAxis(angles[5], Vector3.forward) *
                Quaternion.AngleAxis(90, Vector3.down);
            rightUpper.localRotation = Quaternion.Inverse(core.localRotation) * chain;                

            //Right Fore (Animation) Left Fore (User) node 4    
            chain = Quaternion.AngleAxis(angles[11], Vector3.down) *
                Quaternion.AngleAxis(angles[10], Vector3.left) *
                Quaternion.AngleAxis(angles[9], Vector3.forward) *
                Quaternion.AngleAxis(90, Vector3.down);
            rightFore.localRotation = Quaternion.Inverse(rightUpper.localRotation) *
                Quaternion.Inverse(core.localRotation) * chain;

            //Left Upper (Animation) Right Upper (User) node 3
            chain = Quaternion.AngleAxis(angles[15], Vector3.down) *
                Quaternion.AngleAxis(angles[14], Vector3.left) *
                Quaternion.AngleAxis(angles[13], Vector3.forward) *
                Quaternion.AngleAxis(90, Vector3.up);
            leftUpper.localRotation = Quaternion.Inverse(core.localRotation) * chain;

            //Left Fore (Animation) Right Fore (User) node 5
            chain = Quaternion.AngleAxis(angles[19], Vector3.down) *
                Quaternion.AngleAxis(angles[18], Vector3.left) *
                Quaternion.AngleAxis(angles[17], Vector3.forward) *
                Quaternion.AngleAxis(90, Vector3.up);
            leftFore.localRotation = Quaternion.Inverse(leftUpper.localRotation) *
                Quaternion.Inverse(core.localRotation) * chain;
        }
    }
}