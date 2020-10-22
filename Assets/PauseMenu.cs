using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameManager gameManager;
    public GameObject button;


    public bool isBackButton = false;
    int state = 0;
    string[] states = { "Menu", "Close" };

    // Update is called once per frame
    public void togglePauseMenu()
    {
        if (isBackButton)
        {
            Transform screens = pauseMenu.transform.Find("Screens");
            for (int i = 0; i < screens.childCount; i++)
            {
                if (screens.GetChild(i).gameObject.activeSelf)
                {
                    screens.GetChild(i).gameObject.SetActive(false);
                }
            }
            isBackButton = false;
            state = (state + 1) % 2;
            button.GetComponent<Text>().text = states[state];
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        else
        {
            state = (state + 1) % 2;
            button.GetComponent<Text>().text = states[state];
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            gameManager.togglePause();
        }
    }

    public void toggleBackButton()
    {
        isBackButton = !isBackButton;
    }
}
