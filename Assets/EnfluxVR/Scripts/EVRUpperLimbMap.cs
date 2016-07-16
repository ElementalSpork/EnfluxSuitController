using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EVRUpperLimbMap : EVRHumanoidLimbMap, ILimbAnimator {
    JointRotations jointRotations = new JointRotations();
    private Quaternion chain;

    private float[] initCore = new float[] { 0, 0, 0, 0 };
    private float[] initLeftUpper = new float[] { 0, 0, 0, 0 };
    private float[] initLeftFore = new float[] { 0, 0, 0, 0 };
    private float[] initRightUpper = new float[] { 0, 0, 0, 0 };
    private float[] initRightFore = new float[] { 0, 0, 0, 0 };
    private Quaternion initCorePose = new Quaternion();
    private Quaternion initRightUpperPose = new Quaternion();
    private Quaternion initRightForePose = new Quaternion();
    private Quaternion initLeftUpperPose = new Quaternion();
    private Quaternion initLeftForePose = new Quaternion();
    private Queue<Quaternion> corePose = new Queue<Quaternion>();
    private Queue<Quaternion> rightUpperPose = new Queue<Quaternion>();
    private Queue<Quaternion> rightForePose = new Queue<Quaternion>();
    private Queue<Quaternion> leftUpperPose = new Queue<Quaternion>();
    private Queue<Quaternion> leftForePose = new Queue<Quaternion>();


    public void setInit()
    {
        initState = InitState.INIT;
        StartCoroutine(setPoses());
        
    }

    public void resetInit()
    {
        initState = InitState.PREINIT;
        StopAllCoroutines();
    }

    private void setInitRot()
    {
        initCorePose = Quaternion.AngleAxis(initCore[3], Vector3.up) *
            Quaternion.AngleAxis(initCore[2], Vector3.left) *
            Quaternion.AngleAxis(initCore[1], Vector3.back);

        initRightUpperPose = Quaternion.AngleAxis(initRightUpper[3], Vector3.up) *
                Quaternion.AngleAxis(initRightUpper[2], Vector3.left) *
                Quaternion.AngleAxis(initRightUpper[1], Vector3.back) *
                Quaternion.AngleAxis(270, Vector3.up);
    }

    private IEnumerator setPoses()
    {
        while (true)
        {
            if(corePose.Count > 0)
            {
                core.localRotation = corePose.Dequeue();
            }

            if(rightUpperPose.Count > 0)
            {
                rightUpper.localRotation = Quaternion.Lerp(rightUpper.localRotation, rightUpperPose.Dequeue(), .2f);                               
            }

            if (rightForePose.Count > 0)
            {
                rightFore.localRotation = Quaternion.Lerp(rightFore.localRotation, rightForePose.Dequeue(), .2f);
            }

            if(leftUpperPose.Count > 0)
            {
                leftUpper.localRotation = Quaternion.Lerp(leftUpper.localRotation, leftUpperPose.Dequeue(), .2f);
            }

            if (leftForePose.Count > 0)
            {
                leftFore.localRotation = Quaternion.Lerp(leftFore.localRotation, leftForePose.Dequeue(), .2f);
            }

            yield return null;
        }
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

            setInitRot();

        } else if (initState == InitState.INIT && angles != null)
        {
            //do main animating            
            chain = Quaternion.Inverse(initCorePose) * 
                Quaternion.AngleAxis(angles[3], Vector3.up) *
                Quaternion.AngleAxis(angles[2], Vector3.left) *
                Quaternion.AngleAxis(angles[1], Vector3.back);

            corePose.Enqueue(chain);

            //Left Upper user node 2
            //90 deg transform puts sensor in correct orientation
            chain = Quaternion.Inverse(core.localRotation) * 
                Quaternion.AngleAxis(angles[7], Vector3.up) *
                Quaternion.AngleAxis(angles[6], Vector3.left) *
                Quaternion.AngleAxis(angles[5], Vector3.back) *
                Quaternion.AngleAxis(270, Vector3.down);

            leftUpperPose.Enqueue(chain);

            //Left Fore node 4    
            chain = Quaternion.Inverse(leftUpper.localRotation) *
                Quaternion.Inverse(core.localRotation) *
                Quaternion.AngleAxis(angles[11], Vector3.up) *
                Quaternion.AngleAxis(angles[10], Vector3.left) *
                Quaternion.AngleAxis(angles[9], Vector3.back) *
                Quaternion.AngleAxis(270, Vector3.down);

            leftForePose.Enqueue(chain);

            //Right Upper node 3
            chain = Quaternion.Inverse(core.localRotation) * 
                Quaternion.AngleAxis(angles[15], Vector3.up) *
                Quaternion.AngleAxis(angles[14], Vector3.left) *
                Quaternion.AngleAxis(angles[13], Vector3.back) *
                Quaternion.AngleAxis(270, Vector3.up);

            rightUpperPose.Enqueue(chain);            

            //Right Fore (Animation) Right Fore (User) node 5
            chain = Quaternion.Inverse(rightUpper.localRotation) *
                Quaternion.Inverse(core.localRotation) *
                Quaternion.AngleAxis(angles[19], Vector3.up) *
                Quaternion.AngleAxis(angles[18], Vector3.left) *
                Quaternion.AngleAxis(angles[17], Vector3.back) *
                Quaternion.AngleAxis(270, Vector3.up);

            rightForePose.Enqueue(chain);
           
        }
    }
}