using UnityEngine;
using System.Collections;

public static class TestRotations{


    public static Quaternion rotateRightUpper(float angle)
    {
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

	
}
