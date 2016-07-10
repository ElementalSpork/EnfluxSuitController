using UnityEngine;
using System;

public class EVRUpperLimbMap : EVRHumanoidLimbMap, ILimbAnimator {
    JointRotations jointRotations = new JointRotations();

    private float[] initCore = new float[] { 0, 0, 0, 0 };

    public void setInit()
    {
        initState = InitState.INIT;
    }

    public void resetInit()
    {
        initState = InitState.PREINIT;
    }
    
    //interface method
    //public void operate()
    //{
        
    //}

    //interface method
    public void operate(float[] angles)
    {
        //parse angles
        //apply to upper        
        if (initState == InitState.PREINIT && angles != null)
        {
            //do initialization            
            Array.Copy(angles, 0, initCore, 0, 4);

        } else if (initState == InitState.INIT && angles != null)
        {
            //do main animating            
            core.localRotation = Quaternion.AngleAxis(angles[3] - initCore[3], Vector3.down) *
                Quaternion.AngleAxis(angles[2] - initCore[2], Vector3.left) *
                Quaternion.AngleAxis(angles[1] - initCore[1], Vector3.forward);
        }
    }
}