using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EVRInitializationCounter : MonoBehaviour {

    public int delay = 5;
    private Text timerText;
    private EVRSuitManager _manager;
    private EVRHumanoidLimbMap _limbMap;

	// Use this for initialization
	void Start () {
        timerText = gameObject.GetComponent<Text>();
        _manager = GameObject.Find("EVRSuitManager").GetComponent<EVRSuitManager>();
        _limbMap = GameObject.Find("EVRHumanoidLimbMap").GetComponent<EVRHumanoidLimbMap>();
	}

    public void startInitTimer()
    {
        StartCoroutine(initTimer());
    }

    public void stopAnimation()
    {
        _manager.disableAnimate();
        _limbMap.resetInitialize();
    }

    private IEnumerator initTimer()
    {        
        //delay must be at least 5 seconds
        if (delay < 5)
        {
            delay = 5;
        }

        int count = delay;       

        while (count > -1)
        {
            yield return new WaitForSeconds(1.0f);
            if (count != 0)
            {
                timerText.text = count.ToString();
                 if (count == 2)
                {
                    _manager.enableAnimate();                    
                }
            }
            else
            {
                timerText.text = "Ready!";
                _limbMap.initialize();
            }
            count--;            
        }
    }
}
