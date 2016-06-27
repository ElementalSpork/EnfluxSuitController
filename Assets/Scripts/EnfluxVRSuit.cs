using UnityEngine;
using System.Collections.Generic;
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

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int connectDevices(StringBuilder devices, int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int disconnectDevices(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void performCalibration(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void finishCalibration(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void streamRealTime(int numdevices);

        [DllImport(dllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void stopRealTime(int numdevices);
    }

    public static void connect(List<string> devices)
    {
        StringBuilder apiArg = new StringBuilder();

        for(int device = 0; device < devices.Count; device++)
        {
            apiArg.Append(devices[device]);
            if(device < (devices.Count - 1))
            {
                apiArg.Append(",");
            }
        }
        //api expects input of all address to connect to, seperated by comma
        //example format: XX:XX:XX:XX:XX:XX,YY:YY:YY:YY:YY:YY
        EVRSUIT_0_0_1.connectDevices(apiArg, devices.Count);
    }

    //public static void disconnect()
    //{
    //    EVRSUIT_0_0_1.disconnectDevice();
    //}

    //public static void startStreaming()
    //{
    //    EVRSUIT_0_0_1.startSensorStream();
    //}

    //public static void stopStreaming()
    //{
    //    EVRSUIT_0_0_1.stopSensorStream();
    //}

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
