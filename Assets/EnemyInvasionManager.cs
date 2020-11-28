using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInvasionManager : MonoBehaviour
{

    public List<EnemyRarity> normalEnemies;
    public int numberOfNormalEnemies;
    public List<EnemyRarity> bomberEnemies;
    public int numberOfBombers;
    public float timeTillInvasionMax;
    public float timeTillInvasion;
    public int secondsToStartTimer;
    private List<GameObject> bomberEnemiesToChooseFrom;
    private List<GameObject> normalEnemiesToChooseFrom;
    private Player player;
    public float distanceAwayFromHome;
    public Timer timer;
    // Start is called before the first frame update
    void Start()
    {
        timeTillInvasion = timeTillInvasionMax;
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
            //generateAndSpawnInvasion();
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
    }
}
