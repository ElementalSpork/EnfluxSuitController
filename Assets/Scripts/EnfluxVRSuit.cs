using UnityEngine;
using System.Collections;
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
        public static extern void stopScanPorts();

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void attachPort(StringBuilder port, 
            ScanCallbackDel scb, MessageCallbackDel mcb, StreamCallbackDel strmcb);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void detachPort();
    }

    public static void startScanPorts(IFindPortCallback fcb)
    {
        //gets any avaiable COM ports on PC
        EVRSUIT_0_0_1.startScanPorts(new FindPortCallbackDel(fcb.findportCallback));
    }

    public static void stopScanPorts()
    {
        //stop looking for COM ports
        //todo: remove this, it is no longer exposed in the dll
        EVRSUIT_0_0_1.stopScanPorts();
    }

    public static void attachSelectedPort(StringBuilder port, IOperationCallbacks ocb)
    {
        //attach to a selected COM port, if BlueGiga port then scans for BLE
        //EVRSUIT_0_0_1.attachPort(port, 
        //    new ScanCallbackDel(ocb.scanCallback),
        //    new MessageCallbackDel(ocb.messageCallback),
        //    new StreamCallbackDel(ocb.streamCallback));
        EVRSUIT_0_0_1.attachPort(port, ocb.scanCallback,
            ocb.messageCallback,
            ocb.streamCallback);
    }

    public static void detachPort()
    {
        //detach from whatever port is attached
        EVRSUIT_0_0_1.detachPort();
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
