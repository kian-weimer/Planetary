using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // CONSTANT GAME VARIABLES
    public int numberOfRings; // actually number of planet rings not the number of planets

    public Player player;
    public PlayerController playerController;
    public RandomEventManager REM;
    public Map map;

    public static bool lightsOn = false;
    public static bool popupsOn = false;

    [HideInInspector]
    public int gridResolution = 100; // whaT DOES THIS DO

    public GameObject loadingPanel;
    public Slider loadingBar;
    public Text loadingText;

    public AudioManager AM;
    public BroadcastMessage BM;

    // save refrences
    public LevelTree levelTree;

    public void Update()
    {
        if (Input.GetKeyDown("["))
        {
            levelTree.Save();
            BM.Broadcast("Saved");

        }
        if (Input.GetKeyDown("]"))
        {
            levelTree.Load();
            BM.Broadcast("Loaded");
        }
    }


    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start Menu")
        {
            AM.Play("Main");
        }
        else
        {
            AM.Play("Theme");
        }
    }
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

    // DIFFICULTY PARMS OVERWRITE THE INSPECTOR VALUES
    public void sandbox()
    {
        PlayerPrefs.SetInt("infiniteMoney", 1); // endless money
        PlayerPrefs.SetInt("infiniteSkills", 1); // endless skills

        PlayerPrefs.SetInt("rarityRingDistanceLimit", 25);
        PlayerPrefs.SetInt("frequencyIncrementor", 2);
        PlayerPrefs.SetFloat("fuelConsumption", 1);
        PlayerPrefs.SetInt("maxHealth", 200);
        PlayerPrefs.SetFloat("timeBetweenEvents", 180);
        PlayerPrefs.SetFloat("timeBetweenEnemies", 40);
        PlayerPrefs.SetFloat("upgradeAmount", 1.25f);
        EnterGame();
    }
    public void normal()
    {
        PlayerPrefs.SetInt("rarityRingDistanceLimit", 25); 
        PlayerPrefs.SetInt("frequencyIncrementor", 2); 
        PlayerPrefs.SetFloat("fuelConsumption", 1);
        PlayerPrefs.SetInt("maxHealth", 200);
        PlayerPrefs.SetFloat("timeBetweenEvents", 180);
        PlayerPrefs.SetFloat("timeBetweenEnemies", 40);
        PlayerPrefs.SetFloat("upgradeAmount", 1.25f);

        PlayerPrefs.SetInt("infiniteMoney", 0);
        PlayerPrefs.SetInt("infiniteSkills", 0);

        EnterGame();
    }
    public void hardcore()
    {
        PlayerPrefs.SetInt("rarityRingDistanceLimit", 35); // make rarer planets further away
        PlayerPrefs.SetInt("frequencyIncrementor", 4); // spawn fewer planets 
        PlayerPrefs.SetFloat("fuelConsumption", 1.5f); // player consumes more fuel
        PlayerPrefs.SetInt("maxHealth", 100); // lower starting max health 
        PlayerPrefs.SetFloat("timeBetweenEvents", 120); // more frequent events 
        PlayerPrefs.SetFloat("timeBetweenEnemies", 30); // more enemies spawn 
        PlayerPrefs.SetFloat("upgradeAmount", 1.5f); // upgrades become more expensive faster
        numberOfRings = (int)(numberOfRings*1.5); // allow for further travel 

        PlayerPrefs.SetInt("infiniteMoney", 0);
        PlayerPrefs.SetInt("infiniteSkills", 0);

        EnterGame();
    }

    IEnumerator LoadSceneAsync(string levelName)
    {
        loadingPanel.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(levelName);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);
            // Debug.Log(op.progress);
            loadingBar.value = progress;
            loadingText.text = progress * 100f + "%";

            yield return null;
        }
    }

    public void EnterGame()
    {
        StartCoroutine(LoadSceneAsync("SampleScene"));
    }

    public void Tutorial()
    {
        StartCoroutine(LoadSceneAsync("Tutorial"));
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


