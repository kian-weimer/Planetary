using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{

    public float timeBetweenEvents;
    public float randomEventTimeVariance;

    public RandomEvent[] listOfEvents = new RandomEvent[2];

    private float timeRemaining;
    public bool eventIsHappening = false;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = timeBetweenEvents;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeRemaining <= 0 && !eventIsHappening)
        {
            eventIsHappening = true;
            Instantiate(listOfEvents[0]);

        }
        else
        {
            timeRemaining -= 1;
        }

        if (!eventIsHappening && timeRemaining <= 0)
        {
            timeRemaining = timeBetweenEvents;
        }
    }
}
