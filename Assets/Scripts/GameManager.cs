using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // CONSTANT GAME VARIABLES
    public int numberOfRings; // actually number of planet rings not the number of planets

    public Player player;
    public PlayerController playerController;
    public RandomEventManager REM;

    public static bool lightsOn = false;
    public static bool popupsOn = false;

    [HideInInspector]
    public int gridResolution = 100; // whaT DOES THIS DO


    public void Quit()
    {
        Application.Quit();
    }

    public void togglePause()
    {
        player.gameObject.GetComponent<PlayerController>().speed = 0;
        player.canShoot = !player.canShoot;
        player.disabled = !player.disabled;
        playerController.canMove = !playerController.canMove;
        REM.running = !REM.running;
    }

    public void pause()
    {
        player.gameObject.GetComponent<PlayerController>().speed = 0;
        player.canShoot = false;
        player.disabled = true;
        playerController.canMove = false;
        REM.running = false;
    }

    public void unpause()
    {
        player.canShoot = true;
        player.disabled = false;
        playerController.canMove = true;
        REM.running = true;
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Home()
    {
        SceneManager.LoadScene("Start Menu");
    }


    //**************settings buttons********************
    public void toggleLights()
    {
        if (lightsOn)
        {
            lightsOn = false;
            var lights = FindObjectsOfType<Light2D>();
            foreach(Light2D light in lights)
            {
                light.enabled = false;
            }
        }
        else
        {
            lightsOn = true;
            var lights = FindObjectsOfType<Light2D>();
            foreach (Light2D light in lights)
            {
                light.enabled = true;
            }
        }
    }

    public void togglePopups()
    {
        if (popupsOn)
        {
            popupsOn = false;
        }
        else
        {
            popupsOn = true;
        }
    }
}


