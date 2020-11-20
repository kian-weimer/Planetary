using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialResourceCollection : MonoBehaviour
{
    private float maxTimeBeforeEvents = 6f;
    public float timeBeforeNextText;
    private int tutIndex = 0;
    public List<string> listOfTutorialStrings = new List<string>();
    public bool isWaitingOnBasePlanet = true;
    public bool isWaitingOnComboPlanet = true;
    public bool waitingOnRock = true;
    public GameObject tutorialEnemy;

    public bool hasKilledEnemy;

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
                GetComponent<tutorialGameManager>().updateMessageText("Use the left click mouse button to shoot");
                FindObjectOfType<Player>().canShoot = true;
            }

            else if(tutIndex == 2)
            {
                GetComponent<tutorialGameManager>().updateMessageText("to collect resources explode planets", "by shooting them");
            }

            else if(tutIndex == 3)
            {
                GetComponent<tutorialGameManager>().updateMessageText("First lets collect a rock");
            }

            else if (tutIndex == 4)
            {
                GetComponent<tutorialGameManager>().updateMessageText("to figure out the objective", "check the quest bar on the right side");
            }

            else if (tutIndex == 5)
            {
                GetComponent<tutorialGameManager>().updateMessageText("to turn in some quests you need", "to drag and drop the game object onto the quest area");
                
            }

            else if(tutIndex == 6)
            {
                if (waitingOnRock)
                {
                    tutIndex = 5;
                }
            }

            else if (tutIndex == 7)
            {
                GetComponent<tutorialGameManager>().updateMessageText("After collecting the resources", "you can use them to generate new home planets");
                timeBeforeNextText = timeBeforeNextText * 1.2f;
            }
            else if (tutIndex == 8)
            {
                GetComponent<tutorialGameManager>().updateMessageText("These planets will produce", "resources for you over time");
            }
            else if (tutIndex == 9)
            {
                GetComponent<tutorialGameManager>().updateMessageText("First lets get 3 oxygen resources", "to make an oxygen planet");
            }
            else if (tutIndex == 10)
            {
                GetComponent<tutorialGameManager>().updateMessageText("the right side has a quest bar", "which will show you what you need to do");
            }
            else if(tutIndex == 11)
            {
                GetComponent<tutorialGameManager>().updateMessageText("after collecting 3 oxygens place", "them on one of your home planets and click combo");
                timeBeforeNextText = timeBeforeNextText * 1.2f;
            }
            else if (tutIndex == 12)
            {
                GetComponent<tutorialGameManager>().updateMessageText();
            }
            else if (tutIndex == 13)
            {
                if (isWaitingOnBasePlanet)
                {
                    tutIndex = 12;
                }
            }
            else if(tutIndex == 14)
            {
                GetComponent<tutorialGameManager>().updateMessageText("Now make a combo planet", "1 wood 1 water and 1 oxygen");
            }

            else if (tutIndex == 15)
            {
                GetComponent<tutorialGameManager>().updateMessageText("if you need help figuring out which planet", "is which use the popups option in settings");
                timeBeforeNextText = timeBeforeNextText * 1.2f;
            }

            else if(tutIndex == 16)
            {
                GetComponent<tutorialGameManager>().updateMessageText("", "");
            }
            else if(tutIndex == 17)
            {
                if (isWaitingOnComboPlanet)
                {
                    tutIndex = 16;
                }
                
            }
            else if(tutIndex == 18)
            {
                GetComponent<tutorialGameManager>().updateMessageText("Watch out", "incomming enemy");
                GameObject enemy = Instantiate(tutorialEnemy);
                enemy.transform.position =  new Vector2(FindObjectOfType<Player>().gameObject.transform.position.x + 15, FindObjectOfType<Player>().gameObject.transform.position.y + 15);
            }

            else if (tutIndex == 19)
            {
                GetComponent<tutorialGameManager>().updateMessageText();
                if (!hasKilledEnemy)
                {
                    tutIndex = 18;
                }
                else
                {
                    GetComponent<tutorialGameManager>().updateMessageText("congrats you defeated him");
                }
            }

            else if (tutIndex == 20)
            {
                GetComponent<tutorialGameManager>().updateMessageText("by defeating the enemy and completing", "the quest you gained some experience");
            }

            else if (tutIndex == 21)
            {
                GetComponent<tutorialGameManager>().updateMessageText("If the bar fills up you will", "gain a skill point");
            }

            else if (tutIndex == 22)
            {
                GetComponent<tutorialGameManager>().updateMessageText("Use these skill points in", "the skills section found in the menu");
            }
            else if(tutIndex == 23)
            {
                GetComponent<tutorialGameManager>().updateMessageText();
            }

            else if(tutIndex == 24)
            {
                GetComponent<tutorialGameManager>().updateMessageText("There is also a", "store to buy new upgrades");
            }

            else if (tutIndex == 25)
            {
                GetComponent<tutorialGameManager>().updateMessageText("You have been given", "some money to buy things");
                FindObjectOfType<Money>().addMoney(10000);
            }
            else if (tutIndex == 26)
            {
                GetComponent<tutorialGameManager>().updateMessageText("The store button", "is found in the bottom right");
            }
            else if(tutIndex == 27)
            {
                GetComponent<tutorialGameManager>().updateMessageText();
            }

            else if (tutIndex == 28)
            {
                GetComponent<tutorialGameManager>().updateMessageText("To win the game you must collect The three rarest items", "DarkMatter Diamond and Obsidian");
                timeBeforeNextText = timeBeforeNextText * 1.2f;
            }
            else if (tutIndex == 29)
            {
                GetComponent<tutorialGameManager>().updateMessageText("Have fun playing");
            }
            else if (tutIndex == 30)
            {
                GetComponent<tutorialGameManager>().updateMessageText("quit using the menu by clicking home");
            }
            else if(tutIndex == 35)
            {
                SceneManager.LoadScene("Start Menu");
            }
        }
        else
        {
            timeBeforeNextText -= Time.deltaTime;
        }
    }

    public void createdBasePlanet()
    {
        isWaitingOnBasePlanet = false;
    }

    public void createdComboPlanet()
    {
        isWaitingOnComboPlanet = false;
    }
}
