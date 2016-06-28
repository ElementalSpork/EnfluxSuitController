using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using EnflxStructs;

public class EVRSuitManager : MonoBehaviour
{
    private ComPorts availablePorts;
    private AttachedPort attachedPort;
    public List<string> ports { get { return availablePorts._ports; } }
    public List<string> connectedDevices;
    private ConnectionState operatingState = ConnectionState.NONE;
    private ServerState serverState = ServerState.CLOSED;
    private string host = "localhost";
    private Int32 port = 12900;
    private NetworkStream stream;
    private StreamWriter streamWriter;
    private BinaryReader streamReader;
    private TcpClient client;
    private System.Diagnostics.Process serverProcess;

    private enum ConnectionState
    {
        NONE,
        ATTACHED,
        DETACHED,
        CONNECTED,
        DISCONNECTED,
        CALIBRATING,
        STREAMING
    };

    private enum ServerState
    {
        CLOSED,
        STARTED
    };
    
    void Awake()
    {
        //Get available COM ports
        availablePorts = new ComPorts();
        EnfluxVRSuit.startScanPorts(availablePorts);
    } 
    
    void Start()
    {
        /**
         * required so that when socket server launches, does not pause Unity
         * will go away in future iterations
         * */
        Application.runInBackground = true;
        StartCoroutine(launchServer());
    }

    void OnApplicationQuit()
    {
        //Just in case some steps were skipped
        Debug.Log("Making sure things are closed down");
        /**
         * Skips state check to be certain that port and background thread is
         * shutdown
         * */
        if (operatingState != ConnectionState.NONE && operatingState != ConnectionState.DETACHED)
        {
            EnfluxVRSuit.detachPort();
        }

        if (serverState != ServerState.CLOSED)
        {
            //todo: message from server confirming client disconnect
            //client.Close();
            //todo: make this actually kill the process
            //currently just returns and error
            //serverProcess.Kill();
        }
    }
   
    /**
     * Uses coroutine in order to not block main thread
     * Launches Enflux Java socket server
     * The server processes the sensor data stream
     * and produces orientation angles
     * */
    private IEnumerator launchServer()
    {
        serverProcess = new System.Diagnostics.Process();
        string dir = Path.Combine(Environment.CurrentDirectory, "Assets/Plugins/Sensors");
        string file = Path.Combine(dir, "EVRModuleServer.jar");
        serverProcess.StartInfo.FileName = file;
        if (serverProcess.Start())
        {
            Debug.Log("Socket server started");
        }

        //todo: replace this with message from server
        //confirming connection
        yield return new WaitForSeconds(5);
        client = new TcpClient(host, port);
        stream = client.GetStream();
        //todo: looking into doing this such that encoding is specified
        streamWriter = new StreamWriter(stream);
        //todo: verify that this is correct encoding
        streamReader = new BinaryReader(stream, Encoding.UTF8);
        serverState = ServerState.STARTED;
    }

    /**
     * parse friendly name to find COM port 
     * pass COM port in to connect
     * */
    public void attachPort(string friendlyName)
    {
        if(operatingState == ConnectionState.NONE || operatingState == ConnectionState.DETACHED)
        {
            System.Text.RegularExpressions.Regex toComPort =
            new System.Text.RegularExpressions.Regex(@".? \((COM\d+)\)$");
            if (toComPort.IsMatch(friendlyName.ToString()))
            {
                StringBuilder comName = new StringBuilder()
                    .Append(toComPort.Match(friendlyName.ToString()).Groups[1].Value);
                Debug.Log(comName);
                attachedPort = new AttachedPort();
                if (EnfluxVRSuit.attachSelectedPort(comName, attachedPort) < 1)
                {
                    operatingState = ConnectionState.ATTACHED;
                }else
                {
                    Debug.Log("Error while trying to attach to port: " + comName);
                }
            }
        }else
        {
            Debug.Log("Unable to attach, program is in wrong state "  
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    //api expects input of all address to connect to, seperated by comma
    //example format: XX:XX:XX:XX:XX:XX,YY:YY:YY:YY:YY:YY
    public void connectEnflux(List<string> devices)
    {
        StringBuilder apiArg = new StringBuilder();
        for (int device = 0; device < devices.Count; device++)
        {
            apiArg.Append(devices[device]);
            if (device < (devices.Count - 1))
            {
                apiArg.Append(",");
            }
        }

        if(operatingState == ConnectionState.ATTACHED || 
            operatingState == ConnectionState.DISCONNECTED)
        {
            if (EnfluxVRSuit.connect(apiArg, devices.Count) < 1)
            {
                connectedDevices = devices;
                operatingState = ConnectionState.CONNECTED;
                Debug.Log("Devices connected");
            }
            else
            {
                Debug.Log("Problem connecting");
            }
        }else
        {
            Debug.Log("Unable to connect to devices, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void disconnectEnflux()
    {
        if(operatingState == ConnectionState.CONNECTED)
        {
            if (EnfluxVRSuit.disconnect(connectedDevices.Count) < 1)
            {
                Debug.Log("Devices disconnected");
                client.Close();
                operatingState = ConnectionState.DISCONNECTED;
            }
            else
            {
                Debug.Log("Problem disconnecting");
            }
        }else
        {
            Debug.Log("Unable to disconnect, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void calibrateDevices()
    {
        if(operatingState == ConnectionState.CONNECTED)
        {
            if (EnfluxVRSuit.performCalibration(connectedDevices.Count) < 1)
            {
                operatingState = ConnectionState.CALIBRATING;
            }
            else
            {
                Debug.Log("Problem running calibration");
            }
        }else
        {
            Debug.Log("Unable to calibrate, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void finishCalibration()
    {
        if(operatingState == ConnectionState.CALIBRATING)
        {
            if (EnfluxVRSuit.finishCalibration(connectedDevices.Count) < 1)
            {
                operatingState = ConnectionState.CONNECTED;
            }
            else
            {
                Debug.Log("Problem occured during calibration");
            }
        }else
        {
            Debug.Log("Unable to stop calibration, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void enableAnimate()
    {
        if(operatingState == ConnectionState.CONNECTED)
        {
            if (EnfluxVRSuit.streamRealTime(connectedDevices.Count) < 1)
            {
                operatingState = ConnectionState.STREAMING;
            }
            else
            {
                Debug.Log("Error, no devices to animate");
            }
        }else
        {
            Debug.Log("Unable to stream, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void disableAnimate()
    {
        if(operatingState == ConnectionState.STREAMING)
        {
            if (EnfluxVRSuit.stopRealTime(connectedDevices.Count) > 1)
            {
                operatingState = ConnectionState.CONNECTED;
            }
            else
            {
                Debug.Log("Problem occured while stopping stream");
            }
        }else
        {
            Debug.Log("Unable to stop stream, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }

    public void detachPort()
    {
        if(operatingState == ConnectionState.ATTACHED ||  operatingState == ConnectionState.DISCONNECTED)
        {
            if (EnfluxVRSuit.detachPort() < 1)
            {
                operatingState = ConnectionState.DETACHED;
            }
            else
            {
                Debug.Log("Error occured while detaching");
            }
        }else
        {
            Debug.Log("Unable to detach from port, program is in wrong state "
                + Enum.GetName(typeof(ConnectionState), operatingState));
        }
    }   

    //called on thread created by native dll
    private class ComPorts : EnfluxVRSuit.IFindPortCallback
    {
        public List<string> _ports = new List<string>();

        public void findportCallback(StringBuilder name)
        {
            if (!_ports.Contains(name.ToString()))
            {
                _ports.Add(name.ToString());
            }
        }       
    }

    //called on thread created by native dll
    private class AttachedPort : EnfluxVRSuit.IOperationCallbacks
    {   
        public void messageCallback(sysmsg msgresult)
        {
            Debug.Log(msgresult.msg);
        }

        public void scanCallback(scandata scanresult)
        {
            ThreadDispatch.instance.AddScanItem(scanresult);
        }

        public static void poop(scandata result)
        {
            Debug.Log(result.addr);
        }

        public void streamCallback(streamdata streamresult)
        {
            Debug.Log(streamresult.data);
        }
    }
}
