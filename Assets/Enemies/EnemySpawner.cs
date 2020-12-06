using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<EnemyRarity> enemies;

    public GameObject player;

    public float distanceToSecondLvlEnemy;
    public float distanceToThirdLvlEnemy;

    public float timeBetweenEnemies;
    public float timeBeforeNextEnemy;
    public float enemyDistanceAway;

    public int maxEnemies;
    public List<GameObject> listOfEnemies;
    private Vector3 oldPlayerLocation;
    // Start is called before the first frame update
    void Start()
    {
        loadDifficlutySettings(); // DIFFICULTY PARMS OVERWRITE THE INSPECTOR VALUES
        timeBeforeNextEnemy = timeBetweenEnemies;
        oldPlayerLocation = new Vector3(0, 0, 0);
    }

    public void loadDifficlutySettings() // DIFFICULTY PARMS OVERWRITE THE INSPECTOR VALUES
    {
        if (PlayerPrefs.HasKey("timeBetweenEnemies"))
        {
            timeBetweenEnemies = PlayerPrefs.GetFloat("timeBetweenEnemies");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBeforeNextEnemy <= 0)
        {
            float distanceFromHome = Mathf.Sqrt(Mathf.Pow(player.transform.position.x,2) + Mathf.Pow(player.transform.position.y, 2));
            int rarityLevel = 1;
            //make rarity go down
            if(distanceFromHome >= distanceToSecondLvlEnemy)
            {
                rarityLevel = 2;
            }
            if(distanceFromHome >= distanceToThirdLvlEnemy)
            {
                rarityLevel = 3;
            }
            generateEnemy(rarityLevel);
            timeBeforeNextEnemy = timeBetweenEnemies;
        }
        else if (player.GetComponent<Player>().isHome == false)
        {
            timeBeforeNextEnemy -= Time.deltaTime;
        }
    }

    public void generateEnemy(int rarityLevel)
    {
        
        if (FindObjectOfType<Player>().gameObject.transform.position == oldPlayerLocation)
        {
            return;
        }

        if (maxEnemies == listOfEnemies.Count)
        {
            GameObject enemyToDelete = listOfEnemies[0];
            listOfEnemies = listOfEnemies.GetRange(1, listOfEnemies.Count - 1);
            Destroy(enemyToDelete);
        }

        float degree = Random.Range(0f, 360f);
        float xOffset = Mathf.Cos(degree * Mathf.PI / 180) * enemyDistanceAway;
        float yOffset = Mathf.Sin(degree * Mathf.PI / 180) * enemyDistanceAway;

        List<GameObject> listOfEnemiesToChooseFrom = new List<GameObject>();
        foreach(EnemyRarity enemy in enemies)
        {
            if (enemy.rarityLevel <= rarityLevel)
            {
                for (int j = 0; j < enemy.rarityOfEnemy; j++)
                {
                    listOfEnemiesToChooseFrom.Add(enemy.Enemy);
                }
            }
        }
        int randomIndex = Random.Range(0, listOfEnemiesToChooseFrom.Count);
        GameObject badGuy = Instantiate(listOfEnemiesToChooseFrom[randomIndex]);
        badGuy.transform.position = new Vector2(player.transform.position.x + xOffset, player.transform.position.y + yOffset);
        float distanceAway = badGuy.transform.position.magnitude;
        if(distanceAway < 70)
        {
            Destroy(badGuy);
            return;
        }

        listOfEnemies.Add(badGuy);
        oldPlayerLocation = FindObjectOfType<Player>().gameObject.transform.position;
    }
}
