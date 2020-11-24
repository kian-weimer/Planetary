using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSprintAtPlayer : MonoBehaviour
{
    public float maxTimeBeforeBoost;
    public float timeUntilBoost;
    public Player player;
    private Vector3 playerLocation = new Vector3(0,0,0);
    public float maxSpeed;
    public float thrustSpeed;
    public float startingSped;
    // Start is called before the first frame update
    void Start()
    {
        timeUntilBoost = maxTimeBeforeBoost;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyController>().inTargetingRange)
        {
            timeUntilBoost -= Time.deltaTime;
        }

        if (timeUntilBoost <= 0 && playerLocation.x == 0 && playerLocation.y == 0)
        { 
            playerLocation = player.gameObject.transform.position;
            Debug.Log(playerLocation);
            Debug.Log("hey");
            GetComponent<EnemyController>().turnSpeed = 0;
            //boost
            GetComponent<EnemyController>().maxSpeed = maxSpeed;
            GetComponent<EnemyController>().thrust = thrustSpeed;
            GetComponent<EnemyController>().speed = startingSped;
        }
        if((gameObject.transform.position.x >= playerLocation.x - 1 && gameObject.transform.position.x <= playerLocation.x + 1) && (gameObject.transform.position.y >= playerLocation.y - 1 && gameObject.transform.position.y <= playerLocation.y + 1))
        {
            GetComponent<EnemyController>().turnSpeed = 2;
            //boost
            GetComponent<EnemyController>().maxSpeed = 0;
            GetComponent<EnemyController>().thrust = 0;
            GetComponent<EnemyController>().speed = 0;
            playerLocation = new Vector3(0, 0, 0);
            timeUntilBoost = maxTimeBeforeBoost;
        }
    }
}
