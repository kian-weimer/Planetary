using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSprintAtPlayer : MonoBehaviour
{
    public float maxTimeBeforeBoost;
    public float timeUntilBoost;
    public Player player;
    public float maxSpeed;
    public bool inTargetingRange;
    private bool isBoosting;
    private Vector3 oldLocation;
    public float distanceToBoost;
    public float maxHealth;
    public float health;
    public Weapon weapon;
    private AudioManager audioManager;
    private QuestSystem questSystem;
    public GameObject deathExplosion;
    public GameObject expPoint;
    private bool gettingReadyToBoost = false;
    // Start is called before the first frame update
    void Start()
    {
        questSystem = FindObjectOfType<QuestSystem>();
        audioManager = FindObjectOfType<AudioManager>();
        weapon = Instantiate(weapon);
        weapon.transform.parent = transform;
        
        health = maxHealth;
        timeUntilBoost = maxTimeBeforeBoost;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inTargetingRange)
        {
            timeUntilBoost -= Time.deltaTime;
            if (!isBoosting)
            {
                if(weapon.Shoot(Vector2.zero, Vector2.zero, Vector2.zero))
                {
                    audioManager.Play("LazerShoot");
                }
            }
        }

        if(timeUntilBoost <= .75f && !gettingReadyToBoost)
        {
            Vector3 playerRelativePosition = player.transform.position - transform.position;
            float playerDirection = Mathf.Rad2Deg * Mathf.Atan(playerRelativePosition.y / playerRelativePosition.x);
            if (playerRelativePosition.x > 0) { playerDirection += 180f; }

            GetComponent<Rigidbody2D>().velocity = playerRelativePosition.normalized * maxSpeed / 20f;
            gettingReadyToBoost = true;
        }

        if (timeUntilBoost <= 0 && !isBoosting)
        { 
            Vector3 playerRelativePosition = player.transform.position - transform.position;
            float playerDirection = Mathf.Rad2Deg * Mathf.Atan(playerRelativePosition.y / playerRelativePosition.x);
            if (playerRelativePosition.x > 0) { playerDirection += 180f; }

            GetComponent<Rigidbody2D>().velocity = playerRelativePosition.normalized * maxSpeed;

            GetComponent<Rigidbody2D>().rotation = playerDirection + 90;
            isBoosting = true;
            oldLocation = transform.position;
        }
        else if(!isBoosting)
        {
            Vector3 playerRelativePosition = player.transform.position - transform.position;
            float playerDirection = Mathf.Rad2Deg * Mathf.Atan(playerRelativePosition.y / playerRelativePosition.x);
            if (playerRelativePosition.x > 0) { playerDirection += 180f;}
            GetComponent<Rigidbody2D>().rotation = playerDirection + 90;
        }

        if((oldLocation - transform.position).magnitude > distanceToBoost && isBoosting)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
            isBoosting = false;
            timeUntilBoost = maxTimeBeforeBoost;
            gettingReadyToBoost = false;
        }
    }

    public void death()
    {

        GetComponent<Boss>().died();
        GameObject exp = Instantiate(deathExplosion);
        exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        spawnExp(5);
        questSystem.updateQuestsEnemy(gameObject.name);
        Destroy(gameObject);
    }

    public void spawnExp(int expCount)
    {
        for (int i = 0; i < expCount; i++)
        {
            GameObject exp = Instantiate(expPoint);
            exp.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            Vector2 velocityDirection = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
            }

            exp.GetComponent<Rigidbody2D>().velocity = velocityDirection;
            exp.GetComponent<Rigidbody2D>().angularVelocity = 720;
        }
    }
}
