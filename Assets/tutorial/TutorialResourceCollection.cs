using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialResourceCollection : MonoBehaviour
{
    private float maxTimeBeforeEvents = 4.5f;
    private float timeBeforeNextText;
    private int tutIndex = 0;
    public List<string> listOfTutorialStrings = new List<string>();
    public bool isWaitingOnBasePlanet = true;
    public bool isWaitingOnComboPlanet = true;
    public GameObject tutorialEnemy;
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
                GetComponent<tutorialGameManager>().updateMessageText("the right side shows you an", "image of what you need");
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
                if (isWaitingOnBasePlanet)
                {
                    tutIndex = 7;
                }
            }
            else if(tutIndex == 9)
            {
                GetComponent<tutorialGameManager>().updateMessageText("Now make a combo planet", "1 wood 1 water and 1 oxygen");
            }
            else if(tutIndex == 11)
            {
                GetComponent<tutorialGameManager>().updateMessageText("", "");
            }
            else if(tutIndex == 12)
            {
                if (isWaitingOnComboPlanet)
                {
                    tutIndex = 11;
                }
                
            }
            else if(tutIndex == 13)
            {
                GetComponent<tutorialGameManager>().updateMessageText("Watch out", "incomming enemy");
                GameObject enemy = Instantiate(tutorialEnemy);
                enemy.transform.position =  new Vector2(FindObjectOfType<Player>().gameObject.transform.position.x + 15, FindObjectOfType<Player>().gameObject.transform.position.y);
            }

            else if (tutIndex == 14)
            {
                GetComponent<tutorialGameManager>().updateMessageText();
            }

            else if (tutIndex == 16)
            {
                GetComponent<tutorialGameManager>().updateMessageText("After defeating the enemy", "you will gain experience");
            }

            else if (tutIndex == 17)
            {
                GetComponent<tutorialGameManager>().updateMessageText("If the bar fills up you will", "gain a skill point");
            }

            else if (tutIndex == 18)
            {
                GetComponent<tutorialGameManager>().updateMessageText("Use these skill points in", "the skills section found in the menu");
            }
            else if(tutIndex == 19)
            {
                GetComponent<tutorialGameManager>().updateMessageText();
            }

            else if(tutIndex == 20)
            {
                GetComponent<tutorialGameManager>().updateMessageText("There is also a", "store to buy new upgrades");
            }

            else if (tutIndex == 21)
            {
                GetComponent<tutorialGameManager>().updateMessageText("You have been given", "some money to buy things");
                FindObjectOfType<Money>().addMoney(1000);
            }
            else if (tutIndex == 22)
            {
                GetComponent<tutorialGameManager>().updateMessageText("The store button", "is found in the bottom right");
            }
            else if(tutIndex == 23)
            {
                GetComponent<tutorialGameManager>().updateMessageText();
            }

            else if (tutIndex == 25)
            {
                GetComponent<tutorialGameManager>().updateMessageText("To win the game you must collect The three rarest items", "DarkMatter Diamond and Obsidian");
            }
            else if (tutIndex == 26)
            {
                GetComponent<tutorialGameManager>().updateMessageText("Have fun playing");
            }
            else if (tutIndex == 27)
            {
                GetComponent<tutorialGameManager>().updateMessageText("quit using the menu by clicking home");
            }
        }
        else
        {
            timeBeforeNextText -= Time.deltaTime;
        }
    }

    public void createdBasePlanet()
    {
        GetComponent<tutorialGameManager>().enablePlanetImage(3);
        isWaitingOnBasePlanet = false;
    }

    public void createdComboPlanet()
    {
        isWaitingOnComboPlanet = false;
        GetComponent<tutorialGameManager>().enablePlanetImage(-1);
    }
}
