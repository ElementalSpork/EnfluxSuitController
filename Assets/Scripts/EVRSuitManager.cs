using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using EnflxStructs;

public class EVRSuitManager : MonoBehaviour
{
    private ComPorts availablePorts;
    private AttachedPort attachedPort;
    public List<string> ports { get { return availablePorts._ports; } }
    
    void Awake()
    {
        //Get available COM ports
        availablePorts = new ComPorts();
        EnfluxVRSuit.startScanPorts(availablePorts);
        attachedPort = new AttachedPort();
    }   

    /**
     * parse friendly name to find COM port 
     * pass COM port in to connect
     * */
    public void attachPort(string friendlyName)
    {
        System.Text.RegularExpressions.Regex toComPort =
            new System.Text.RegularExpressions.Regex(@".? \((COM\d+)\)$");
        if (toComPort.IsMatch(friendlyName.ToString()))
        {
            StringBuilder comName = new StringBuilder()
                .Append(toComPort.Match(friendlyName.ToString()).Groups[1].Value);
            Debug.Log(comName);
            EnfluxVRSuit.attachSelectedPort(comName, attachedPort);
        }
    }

    public void detachPort()
    {
        EnfluxVRSuit.detachPort();
    }

    public void changeText()
    {
        Debug.Log("Hi");
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
