//========= Copyright 2016, Enflux Inc. All rights reserved. ===========
//
// Purpose: Interface to native EnfluxVR plugin
//
//======================================================================

using System.Text;
using System.Runtime.InteropServices;
using EnflxStructs;

internal static class EnfluxVRSuit {

    private const string dllName = "ModuleInterface";
    private delegate void ScanCallbackDel(scandata scanresult);
    private delegate void StreamCallbackDel(streamdata streamresult);
    private delegate void MessageCallbackDel(sysmsg msgresult);
    private delegate void FindPortCallbackDel(StringBuilder buffer);

    private static class EVRSUIT_0_0_1
    {
        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void startScanPorts(FindPortCallbackDel fcb);        

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int attachPort(StringBuilder port, 
            ScanCallbackDel scb, MessageCallbackDel mcb, StreamCallbackDel strmcb);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int detachPort();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int connectDevices(StringBuilder devices, int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int disconnectDevices(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int performCalibration(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int finishCalibration(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int streamRealTime(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int stopRealTime(int numdevices);
    }

    public static int connect(StringBuilder devices, int numdevices)
    {   
        return EVRSUIT_0_0_1.connectDevices(devices, numdevices);
    }

    public static int disconnect(int numdevices)
    {
        return EVRSUIT_0_0_1.disconnectDevices(numdevices);
    }

    public static int performCalibration(int numdevices)
    {
        return EVRSUIT_0_0_1.performCalibration(numdevices);
    }

    public static int finishCalibration(int numdevices)
    {
        return EVRSUIT_0_0_1.finishCalibration(numdevices);
    }

    public static int streamRealTime(int numdevices)
    {
        return EVRSUIT_0_0_1.streamRealTime(numdevices);
    }

    public static int stopRealTime(int numdevices)
    {
        return EVRSUIT_0_0_1.stopRealTime(numdevices);
    }

    public static void startScanPorts(IFindPortCallback fcb)
    {
        //gets any avaiable COM ports on PC
        EVRSUIT_0_0_1.startScanPorts(new FindPortCallbackDel(fcb.findportCallback));
    }    

    public static int attachSelectedPort(StringBuilder port, IOperationCallbacks ocb)
    {
        //attach to a selected COM port, if BlueGiga port then scans for BLE
        return EVRSUIT_0_0_1.attachPort(port,
            new ScanCallbackDel(ocb.scanCallback),
            new MessageCallbackDel(ocb.messageCallback),
            new StreamCallbackDel(ocb.streamCallback));
    }

    public static int detachPort()
    {
        return EVRSUIT_0_0_1.detachPort();
    }

    public interface IFindPortCallback
    {
        void findportCallback(StringBuilder buffer);
    }

    //Callbacks to support device operations
    public interface IOperationCallbacks
    {
        //results from scanning such as address, name, and rssi
        void scanCallback(scandata scanresult);
        //streaming data
        void streamCallback(streamdata streamresult);
        //system messages such as state or errors
        void messageCallback(sysmsg msgresult);
    }
}
