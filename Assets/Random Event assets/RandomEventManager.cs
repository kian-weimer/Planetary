﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventManager : MonoBehaviour
{

    public float timeBetweenEvents = 30;
    public float randomEventTimeVariance;

    public GameObject[] listOfEvents = new GameObject[1];
    public string[] listOfAlertTexts = new string[1];

    public float timeBeforeNextEvent;

    public GameObject alertText;

    public bool running = true;

    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextEvent = timeBetweenEvents;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBeforeNextEvent <= 0)
        {
            int indexOfEvent = Random.Range(0, listOfEvents.Length);
            GameObject randomEvent = Instantiate(listOfEvents[indexOfEvent]);
            GameObject alert = Instantiate(alertText);
            alert.GetComponent<Text>().text = listOfAlertTexts[indexOfEvent];
            timeBeforeNextEvent = timeBetweenEvents;
        }


        else if(FindObjectOfType<Player>().isHome == false && running)
        {
            timeBeforeNextEvent -= Time.deltaTime;
        }
    }
}
