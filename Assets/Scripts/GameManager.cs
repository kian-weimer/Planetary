using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // CONSTANT GAME VARIABLES
    public int numberOfRings; // actually number of planet rings not the number of planets

    public Player player;
    public PlayerController playerController;
    public RandomEventManager REM;

    [HideInInspector]
    public int gridResolution = 100; // whaT DOES THIS DO

    public void Quit()
    {
        Application.Quit();
    }

    public void togglePause()
    {
        player.canShoot = !player.canShoot;
        playerController.canMove = !playerController.canMove;
        REM.running = !REM.running;
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}


