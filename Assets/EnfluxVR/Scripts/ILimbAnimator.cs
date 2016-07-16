//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Animation interface for EnfluxVR suit
//
//======================================================================

public interface ILimbAnimator {
    //void operate();
    void operate(float[] angles);
    void setInit();
    void resetInit();
}
