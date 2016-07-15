using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnflxStructs;

public class ThreadDispatch : IDispatcher
{
    private Dictionary<string, BleDevice> scanResults = new Dictionary<string, BleDevice>();    
    private static ThreadDispatch _instance;    
    private string deviceName = "Enfl";    

    public static ThreadDispatch instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new ThreadDispatch();
            }

            return _instance;
        }
    }

    public interface IGetScanResults
    {
        void getScanResults();
    }

    public void AddScanItem(scandata item)
    {
        lock (scanResults)
        {
            if (item.name == deviceName)
            {
                if (scanResults.ContainsKey(item.addr))
                {
                    scanResults[item.addr].rssi = item.rssi;                    
                }
                else
                {
                    scanResults.Add(item.addr,
                        new BleDevice(item.addr, item.rssi, item.name));                    
                }
            }
        }
    }

    public Dictionary<string, BleDevice> GetScanItems()
    {
        lock (scanResults)
        {
            if(scanResults.Count > 0)
            {
                //returning copy because that seeems safer
                var dictionary = scanResults.ToDictionary(entry => entry.Key,
                    entry => entry.Value);                
                return dictionary;
            }else
            {
                return null;
            }
        }
    }    
}
