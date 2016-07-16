//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Structs for callbacks fromEnflux native plugin
//
//======================================================================

namespace EnflxStructs
{
    public struct sysmsg
    {
        public string msg;
    }

    public struct scandata
    {
        public string addr;
        public string rssi;
        public string name;
    }

    public struct streamdata
    {
        public string data;
    }
}

