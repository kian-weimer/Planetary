using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.PlayerLoop;
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
    public QuestSystem questSystem;
    public ShopManager shopManager;
    public planetGenerator planetGenerator;
    public Alminac alminac;
    public static bool loadingFromSave = false;
    public Money money;
    public Inventory inventory;
    public Home home;

    public Button loadButton;
    public GameObject warning;
   

    public void Save()
    {
        PlayerPrefs.SetInt("GameSaved", 1);
        player.Save(); // must be before levelTree
        levelTree.Save();
        questSystem.Save();
        shopManager.Save();
        planetGenerator.Save();
        money.Save();
        shopManager.GetComponent<ResourceInventory>().Save();
        alminac.Save();
        inventory.Save();
        if (popupsOn)
        {
            PlayerPrefs.SetInt("popupsOn", 1);
        }
        else
        {
            PlayerPrefs.SetInt("popupsOn", 0);
        }

        if (lightsOn)
        {
            PlayerPrefs.SetInt("lightsOn", 1);
        }
        else
        {
            PlayerPrefs.SetInt("lightsOn", 0);
        }

        map.Save();
        home.Save();
        BM.Broadcast("Saved");
        player.Save();
        FindObjectOfType<EnemyInvasionManager>().Save();
        MaxItemsManager.Save();
        player.gameObject.GetComponent<PlayerController>().Save();
    }

    public void Load()
    {
        shopManager.GetComponent<ResourceInventory>().Load();
        player.Load(); // must be before levelTree
        levelTree.Load();
        questSystem.loadQuests = true;//Load();
        shopManager.Load();
        money.Load();

        if (loadingFromSave) // will only do this if loading at start. only here to help dev testing
        {
            home.Load();
            planetGenerator.Load();
            //loadingFromSave = false;
        }
        inventory.Load();
        alminac.Load();

        if(PlayerPrefs.GetInt("lightsOn") == 1)
        {
            toggleLights();
        }
        if (PlayerPrefs.GetInt("popupsOn") == 1)
        {
            togglePopups();
        }

        map.Load();
        BM.Broadcast("Loaded");
        player.Load();
        FindObjectOfType<EnemyInvasionManager>().Load();
        MaxItemsManager.Load();
        player.gameObject.GetComponent<PlayerController>().Load();
    }

    public void displayWarning()
    {
        warning.SetActive(PlayerPrefs.GetInt("GameSaved") == 1);
    }


    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "Start Menu")
        {
            AM.Play("Main");
            if (PlayerPrefs.GetInt("GameSaved") == 0)
            {
                loadButton.interactable = false;
            }
        }
        else
        {
            AM.Play("Theme");
            if (loadingFromSave)
            {
                Load();
            }
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
        PlayerPrefs.SetFloat("timeBetweenEnemies", 30);
        PlayerPrefs.SetFloat("upgradeAmount", 1.25f);


        PlayerPrefs.SetFloat("maxTime", 800);
        PlayerPrefs.SetFloat("minTime", 600);
        PlayerPrefs.SetInt("numberOfNormals", 4);
        PlayerPrefs.SetInt("numberOfBombers", 2);
        PlayerPrefs.SetInt("numberOfWaves", 3);
        EnterGame();
    }
    public void normal()
    {
        PlayerPrefs.SetInt("rarityRingDistanceLimit", 25); 
        PlayerPrefs.SetInt("frequencyIncrementor", 2); 
        PlayerPrefs.SetFloat("fuelConsumption", 1);
        PlayerPrefs.SetInt("maxHealth", 200);
        PlayerPrefs.SetFloat("timeBetweenEvents", 180);
        PlayerPrefs.SetFloat("timeBetweenEnemies", 30);
        PlayerPrefs.SetFloat("upgradeAmount", 1.25f);

        PlayerPrefs.SetInt("infiniteMoney", 0);
        PlayerPrefs.SetInt("infiniteSkills", 0);

        PlayerPrefs.SetFloat("maxTime", 600);
        PlayerPrefs.SetFloat("minTime", 420);
        PlayerPrefs.SetInt("numberOfNormals", 4);
        PlayerPrefs.SetInt("numberOfBombers", 2);
        PlayerPrefs.SetInt("numberOfWaves", 3);

        EnterGame();
    }
    public void hardcore()
    {
        PlayerPrefs.SetInt("rarityRingDistanceLimit", 35); // make rarer planets further away
        PlayerPrefs.SetInt("frequencyIncrementor", 4); // spawn fewer planets 
        PlayerPrefs.SetFloat("fuelConsumption", 1.5f); // player consumes more fuel
        PlayerPrefs.SetInt("maxHealth", 100); // lower starting max health 
        PlayerPrefs.SetFloat("timeBetweenEvents", 120); // more frequent events 
        PlayerPrefs.SetFloat("timeBetweenEnemies", 20); // more enemies spawn 
        PlayerPrefs.SetFloat("upgradeAmount", 1.5f); // upgrades become more expensive faster
        numberOfRings = (int)(numberOfRings*1.5); // allow for further travel 

        PlayerPrefs.SetInt("infiniteMoney", 0);
        PlayerPrefs.SetInt("infiniteSkills", 0);

        PlayerPrefs.SetFloat("maxTime", 500);
        PlayerPrefs.SetFloat("minTime", 300);
        PlayerPrefs.SetInt("numberOfNormals", 4);
        PlayerPrefs.SetInt("numberOfBombers", 2);
        PlayerPrefs.SetInt("numberOfWaves",3);

        EnterGame();
    }

    public void loadFromSave()
    {
        loadingFromSave = true;
        StartCoroutine(LoadSceneAsync("SampleScene"));
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
        lightsOn = false;
        popupsOn = false;
        PlayerPrefs.SetInt("GameSaved", 0);
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
                if (light.gameObject.name != "Sun")
                {
                    light.enabled = false;
                }
            }
        }
        else
        {
            lightsOn = true;
            var lights = FindObjectsOfType<Light2D>();
            foreach (Light2D light in lights)
            {
                if(light.gameObject.name != "Sun")
                {
                    light.enabled = true;
                }
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


