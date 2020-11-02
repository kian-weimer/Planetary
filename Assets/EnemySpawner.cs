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
    // Start is called before the first frame update
    void Start()
    {
        timeBeforeNextEnemy = timeBetweenEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBeforeNextEnemy <= 0)
        {
            float distanceFromHome = Mathf.Sqrt(Mathf.Pow(player.transform.position.x,2) + Mathf.Pow(player.transform.position.y, 2));
            int rarityLevel = 1;

            //make rarity go down
            rarityLevel = distanceFromHome >= distanceToSecondLvlEnemy ? rarityLevel += 1 : rarityLevel;
            rarityLevel = distanceFromHome >= distanceToThirdLvlEnemy ? rarityLevel += 1 : rarityLevel;

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
        float degree = Random.Range(0f, 360f);
        float xOffset = Mathf.Cos(degree * Mathf.PI / 180) * enemyDistanceAway;
        float yOffset = Mathf.Sin(degree * Mathf.PI / 180) * enemyDistanceAway;

        List<GameObject> listOfEnemiesToChooseFrom = new List<GameObject>();
        foreach(EnemyRarity enemy in enemies)
        {
            if (enemy.rarityLevel >= rarityLevel)
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
        Debug.Log(badGuy.transform.position);
    }
}
