using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorialGameManager : MonoBehaviour
{
    public GameObject upperMessageText;
    public GameObject lowerMessageText;
    public Camera playerCamera;
    public Camera tutorialCamera;
    public TutorialMovement tutMove;
    public List<GameObject> listOfPlanetImages;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera.gameObject.SetActive(false);
        tutorialCamera.gameObject.SetActive(true);
        updateMessageText("This is your home system", "click the mouse to begin");
        upperMessageText.SetActive(true);
    }

    private void FixedUpdate()
    {
        if(tutorialCamera.orthographicSize <= 60)
        {
            tutorialCamera.orthographicSize += Time.deltaTime*3;
        }
        else
        {
            if (lowerMessageText.activeSelf == false)
            {
                lowerMessageText.SetActive(true);
            }
        }

        if(tutorialCamera.gameObject.activeSelf)
        {
            
            if (Input.GetKey(KeyCode.Mouse0))
            {
                tutorialCamera.gameObject.SetActive(false);
                playerCamera.gameObject.SetActive(true);
                updateMessageText("Use A and D to rotate");
                tutMove.canBegin = true;
                lowerMessageText.SetActive(true);
            }
        }
    }
    public void updateMessageText(string message1 = "", string message2 = "")
    {
        upperMessageText.GetComponent<Text>().text = message1;
        lowerMessageText.GetComponent<Text>().text = message2;
    }

    public void enablePlanetImage(int index)
    {
        if(index == -1)
        {
            foreach(GameObject holder in listOfPlanetImages )
            {
                holder.SetActive(true);
            }
        }
        else
        {
            for(int i = 0; i < index; i++)
            {
                listOfPlanetImages[i].SetActive(true);
            }
        }
    }
}
