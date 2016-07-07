using UnityEngine;
using System.Collections;

public interface ILimbAnimator {
    void operate();
    void operate(float[] angles);
}
