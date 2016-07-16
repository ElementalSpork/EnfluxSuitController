//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Interface for dispatching data off background thread
//
//======================================================================

using EnflxStructs;

public interface IDispatcher {
    void AddScanItem(scandata item);
}
