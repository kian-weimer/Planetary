using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // CONSTANT GAME VARIABLES
    public int numberOfRings; // actually number of planet rings not the number of planets

    [HideInInspector]
    public int gridResolution = 100; // whaT DOES THIS DO

    public void Quit()
    {
        Application.Quit();
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}


