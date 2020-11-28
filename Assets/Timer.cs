using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateTime(float timeInSeconds)
    {
        int numberOfMinutes = (int)(timeInSeconds / 60);
        timeInSeconds = timeInSeconds - numberOfMinutes * 60;
        int seconds = (int)timeInSeconds;

        if(seconds < 10)
        {
            gameObject.GetComponent<Text>().text = numberOfMinutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            gameObject.GetComponent<Text>().text = numberOfMinutes.ToString() + ":" + seconds.ToString();
        }
    }

    public void updateTime(int timeInSeconds)
    {
        int numberOfMinutes = (int)(timeInSeconds / 60);
        timeInSeconds = timeInSeconds - numberOfMinutes * 60;
        int seconds = (int)timeInSeconds;

        if (seconds < 10)
        {
            gameObject.GetComponent<Text>().text = numberOfMinutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            gameObject.GetComponent<Text>().text = numberOfMinutes.ToString() + ":" + seconds.ToString();
        }

    }
}
