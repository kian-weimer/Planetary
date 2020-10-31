using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TutorialResourceCollection : MonoBehaviour
{
    private float maxTimeBeforeEvents = 3f;
    private float timeBeforeNextText;
    private int textIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextText = maxTimeBeforeEvents;
    }

    void Update()
    {
        if(timeBeforeNextText <= 0)
        {
            timeBeforeNextText = maxTimeBeforeEvents;
            textIndex += 1;
        }
        else
        {
            timeBeforeNextText -= Time.deltaTime;
        }
    }
}
