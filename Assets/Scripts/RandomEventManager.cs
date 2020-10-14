using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{

    public float timeBetweenEvents = 30;
    public float randomEventTimeVariance;

    public GameObject[] listOfEvents = new GameObject[1];

    private float timeBeforeNextEvent;
    public bool eventIsHappening = false;

    public GameObject alertText;

    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextEvent = timeBetweenEvents;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeBeforeNextEvent <= 0 && !eventIsHappening)
        {
            eventIsHappening = true;
            int indexOfEvent = Random.Range(0, listOfEvents.Length);
            Instantiate(listOfEvents[indexOfEvent]);
            Instantiate(alertText);
            
        }
        else
        {
            timeBeforeNextEvent -= Time.deltaTime;
        }

        /*
        if (!eventIsHappening && timeBeforeNextEvent <= 0)
        {
            timeBeforeNextEvent = timeBetweenEvents;
        }
        */
    }
}
