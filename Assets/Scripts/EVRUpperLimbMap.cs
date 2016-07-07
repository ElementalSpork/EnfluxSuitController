using UnityEngine;
using System.Collections;
using System;

public class EVRUpperLimbMap : EVRHumanoidLimbMap, ILimbAnimator {
    JointRotations jointRotations = new JointRotations();

    public void operate()
    {
        rightUpper.localRotation = jointRotations.rotateRightUpper(45.0f);
    }

    //todo: use this function later
    public void operate(float[] angles)
    {
        //parse angles
        //apply to upper
    }
}
