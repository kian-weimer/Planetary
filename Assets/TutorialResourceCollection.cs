using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TutorialResourceCollection : MonoBehaviour
{
    private float maxTimeBeforeEvents = 3.5f;
    private float timeBeforeNextText;
    private int tutIndex = 0;
    public List<string> listOfTutorialStrings = new List<string>();
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
            tutIndex += 1;
            if(tutIndex == 1)
            {
                GetComponent<tutorialGameManager>().updateMessageText("Use the left click mouse button to shoot", "and explode the planets to collect resources");
                FindObjectOfType<Player>().canShoot = true;
            }
            else if (tutIndex == 2)
            {
                GetComponent<tutorialGameManager>().updateMessageText("After collecting the resources", "you can use them to generate new home planets");
            }
            else if (tutIndex == 3)
            {
                GetComponent<tutorialGameManager>().updateMessageText("These will produce", "resources for you over time");
            }
            else if (tutIndex == 4)
            {
                GetComponent<tutorialGameManager>().updateMessageText("First lets get 3 oxygen resources", "to make an oxygen planet");
                GetComponent<tutorialGameManager>().enablePlanetImage(1);
            }
            else if (tutIndex == 5)
            {
                GetComponent<tutorialGameManager>().updateMessageText("the top right shows you an", "image of what you need");
            }
            else if(tutIndex == 6)
            {
                GetComponent<tutorialGameManager>().updateMessageText("after collecting 3 place them", "on one of your home planets");
            }
            else if (tutIndex == 7)
            {
                GetComponent<tutorialGameManager>().updateMessageText("", "");
            }
            else if (tutIndex == 8)
            {
                tutIndex = 7;
            }
            else if(tutIndex == 9)
            {
                GetComponent<tutorialGameManager>().updateMessageText("First get a wood resource");
            }
        }
        else
        {
            timeBeforeNextText -= Time.deltaTime;
        }
    }

    public void createdPlanet()
    {
        GetComponent<tutorialGameManager>().updateMessageText("Now to make a combo planet", "we need 3 different resources");
        tutIndex = 9;
    }
}
