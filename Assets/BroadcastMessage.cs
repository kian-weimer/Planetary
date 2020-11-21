using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BroadcastMessage : MonoBehaviour
{
    public int displayTime;
    private double startTime;

    public void Broadcast(string message)
    {
        gameObject.SetActive(true);
        transform.Find("Text").GetComponent<Text>().text = message;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + displayTime)
        {
            gameObject.SetActive(false);
        }
    }
}
