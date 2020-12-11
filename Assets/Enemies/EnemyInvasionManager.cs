using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyInvasionManager : MonoBehaviour
{

    public List<EnemyRarity> normalEnemies;
    public int numberOfNormalEnemies;
    public List<EnemyRarity> bomberEnemies;
    public int numberOfBombers;
    public float timeTillInvasionMax;
    public float timeTillInvasionMin;
    public float timeBetweenWaves;
    public int numberOfWaves;
    private int currentWaveNumber = 0;
    private float timeTillWave = 0f;
    public float timeTillInvasion;
    public int secondsToStartTimer;
    private List<GameObject> bomberEnemiesToChooseFrom = new List<GameObject>();
    private List<GameObject> normalEnemiesToChooseFrom = new List<GameObject>();
    private Player player;
    public float distanceAwayFromHome;
    public Timer timer;
    public static bool invasionOccuring = false;
    private static int numberOfInvasionEnemiesAlive = 0;

    public void Save()
    {
        PlayerPrefs.SetFloat("timeTillInvasion",timeTillInvasion);
        PlayerPrefs.SetFloat("timeTillWave", timeTillWave);
    }

    public void Load()
    {
        timeBetweenWaves = PlayerPrefs.GetFloat("timeTillWave");
        timeTillInvasion = PlayerPrefs.GetFloat("timeTillInvasion");
    }

    // Start is called before the first frame update
    void Start()
    {
        timeTillInvasion = Random.Range(timeTillInvasionMin, timeTillInvasionMax);
        loadDifficlutySettings();
        
        player = FindObjectOfType<Player>();
        foreach(EnemyRarity normalEnemy in normalEnemies)
        {
            for(int j = 0; j < normalEnemy.rarityOfEnemy; j ++)
            {
                normalEnemiesToChooseFrom.Add(normalEnemy.Enemy);
            }
        }

        foreach (EnemyRarity bomber in bomberEnemies)
        {
            for (int j = 0; j < bomber.rarityOfEnemy; j++)
            {
                bomberEnemiesToChooseFrom.Add(bomber.Enemy);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeTillInvasion -= Time.deltaTime;

        if(timeTillInvasion <= secondsToStartTimer && !(timeTillInvasion <= 0))
        {
            if (!timer.gameObject.activeSelf)
            {
                timer.gameObject.SetActive(true);
            }
            timer.updateTime(timeTillInvasion);
        }

        if(timeTillInvasion <= 0)
        {
            timer.gameObject.SetActive(false);
            if (timeTillWave <= 0)
            {
                generateAndSpawnInvasion();
                timeTillWave = timeBetweenWaves;
                currentWaveNumber += 1;
            }
            else
            {
                timeTillWave -= Time.deltaTime;
            }
            
            if(currentWaveNumber == numberOfWaves)
            {
                timeTillInvasion = Random.Range(timeTillInvasionMin,timeTillInvasionMax);
                int random = Random.Range(0, 100);
                if(random > 50)
                {
                    numberOfBombers += 1;
                }

                if (random < 50)
                {
                    numberOfNormalEnemies += 1;
                }

                currentWaveNumber = 0;
            }
        } 
    }
    public void generateAndSpawnInvasion()
    {
        for (int i = 0; i < numberOfNormalEnemies; i++)
        {
            int index = Random.Range(0, normalEnemiesToChooseFrom.Count - 1);
            float angle = Random.Range(0f, 360f);

            float xDirection = distanceAwayFromHome * Mathf.Cos(angle);
            float yDirection = distanceAwayFromHome * Mathf.Sin(angle);

            GameObject enemy = Instantiate(normalEnemiesToChooseFrom[index]);
            enemy.transform.position = new Vector2(xDirection, yDirection);

            if(SceneManager.GetActiveScene().name == "Start Menu")
            {
                enemy.GetComponent<EnemyController>().target = FindObjectOfType<BackgroundFriend>().gameObject;
            }
            else
            {
                enemy.GetComponent<EnemyController>().target = player.gameObject;
            }
        }

        for (int i = 0; i < numberOfBombers; i++)
        {
            int index = Random.Range(0, bomberEnemiesToChooseFrom.Count - 1);
            float angle = Random.Range(0f, 360f);

            float xDirection = distanceAwayFromHome * Mathf.Cos(angle);
            float yDirection = distanceAwayFromHome * Mathf.Sin(angle);

            GameObject enemy = Instantiate(bomberEnemiesToChooseFrom[index]);
            enemy.transform.position = new Vector2(xDirection, yDirection);

            /* make the enemy point to home
            Vector3 angleToHome = new Vector3(0,0,0) - transform.position;
            enemy.GetComponent<Rigidbody2D>().rotation = angleToHome.;
            */
        }

        numberOfInvasionEnemiesAlive += (numberOfBombers + numberOfNormalEnemies);

        if (!invasionOccuring)
        {
            invasionOccuring = true;
        }

        normalEnemiesToChooseFrom.Clear();
        foreach (EnemyRarity normalEnemy in normalEnemies)
        {
            for (int j = 0; j < normalEnemy.rarityOfEnemy; j++)
            {
                normalEnemiesToChooseFrom.Add(normalEnemy.Enemy);
            }
        }

        if(numberOfBombers != 0)
        {
            bomberEnemiesToChooseFrom.Clear();
            foreach (EnemyRarity bomber in bomberEnemies)
            {
                for (int j = 0; j < bomber.rarityOfEnemy; j++)
                {
                    bomberEnemiesToChooseFrom.Add(bomber.Enemy);
                }
            }
        }
    }
    public static void lowerCount()
    {
        numberOfInvasionEnemiesAlive -= 1;
        if (numberOfInvasionEnemiesAlive == 0)
        {
            invasionOccuring = false;
            FindObjectOfType<GameManager>().BM.Broadcast("Congratulations you have repelled the attack");
        }
    }

    public void loadDifficlutySettings() // DIFFICULTY PARMS OVERWRITE THE INSPECTOR VALUES
    {
        if (PlayerPrefs.HasKey("numberOfWaves"))
        {
            numberOfWaves = PlayerPrefs.GetInt("numberOfWaves");
        }
        if (PlayerPrefs.HasKey("numberOfBombers"))
        {
            numberOfBombers = PlayerPrefs.GetInt("numberOfBombers");
        }
        if (PlayerPrefs.HasKey("numberOfNormals"))
        {
            numberOfNormalEnemies = PlayerPrefs.GetInt("numberOfNormals");
        }
        if (PlayerPrefs.HasKey("minTime"))
        {
            timeTillInvasionMin = PlayerPrefs.GetInt("minTime");
        }
        if (PlayerPrefs.HasKey("maxTime"))
        {
            timeTillInvasionMax = PlayerPrefs.GetInt("maxTime");
        }
    }
}
