using UnityEngine;
using System.Collections;
using System;

public class EVRUpperLimbMap : EVRHumanoidLimbMap, ILimbAnimator {
    JointRotations jointRotations = new JointRotations();

    public void operate()
    {
        
    }

    //todo: use this function later
    public void operate(float[] angles)
    {
        //parse angles
        //apply to upper
        if(angles != null)
        {
            Debug.Log(angles.Length);
            core.localRotation = Quaternion.AngleAxis(angles[3], Vector3.down) *
            Quaternion.AngleAxis(angles[2], Vector3.left) *
            Quaternion.AngleAxis(angles[1], Vector3.forward);
        }
        

        //core.localRotation = jointRotations.rotateRightUpper(45.0f);

        //rightUpper.localRotation = jointRotations.rotateRightUpper(45.0f);
    }
}