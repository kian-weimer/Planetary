
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public bool inTargetingRange = false;
    public bool inFiringRange = false;
    public bool inStoppingRange = false;
    public GameObject player = null;
    public Rigidbody2D rb;
    public AudioManager audioManager;
    public GameObject weapon;
    public GameObject expPoint;
    public int expPointsDropped;

    public float thrust;
    public float speed;
    public float turnSpeed;
    public float maxSpeed;

    public float health;
    public float maxHealth;
    public float hitDamage;

    public bool enhancedTargeting = false;

    public GameObject deathExplosion;
    public QuestSystem questSystem;

    public GameObject target;
    bool boss = false;
    public bool friendlyThatFollowsPlayer = false;
    public bool invasionEnemy;
    public float distanceAwayToStop;

    public bool isBomber;

    private void Start()
    {
        questSystem = FindObjectOfType<QuestSystem>();
        player = FindObjectOfType<Player>().gameObject;
        audioManager = FindObjectOfType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        if (gameObject.GetComponent<Boss>() != null)
        {
            boss = true;
        }
        if (tag == "Friendly" && friendlyThatFollowsPlayer)
        {
            target = player;
            inTargetingRange = true;
        }

        if(tag == "Friendly")
        {
            transform.SetParent(FindObjectOfType<Home>().transform);
        }

        weapon = Instantiate(weapon);
        weapon.transform.parent = transform;
        health = maxHealth;
    }

    public void damage(float hitPoints)
    {
        health -= hitPoints;
        inTargetingRange = true;
        if (health <= 0)
        {
            if (boss)
            {
                GetComponent<Boss>().died();
            }
            GameObject exp = Instantiate(deathExplosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            spawnExp();
            questSystem.updateQuestsEnemy(gameObject.name);
            Destroy(gameObject);
        }
    }

    public void spawnExp(int expCount = -1)
    {
        if (expCount == -1) { expCount = expPointsDropped; }
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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inFiringRange)
        {
            float rotation = (rb.rotation + 90);
            if (weapon.GetComponent<Weapon>().Shoot(transform.Find("Barrel").position, new Vector2(Mathf.Cos(((rotation) * Mathf.PI) / 180), Mathf.Sin(((rotation) * Mathf.PI) / 180)), gameObject.GetComponent<Rigidbody2D>().velocity))
            {
                audioManager.Play("LazerShoot");
            }
        }

        if (inTargetingRange)
        {
            if (target == null && tag != "Friendly" && !isBomber) { target = player; }
            else if(target == null)
            {
                inTargetingRange = false;
                return;
            }
            Vector3 playerRelativePosition = target.transform.position - transform.position;
            float playerDirection = Mathf.Rad2Deg * Mathf.Atan(playerRelativePosition.y / playerRelativePosition.x);

            if (playerRelativePosition.x > 0) { playerDirection += 180f; }

            if (!inStoppingRange)
            {
                if (speed < maxSpeed)
                {
                    speed += thrust;
                }

                if (enhancedTargeting) // much more accurate, also more predictable
                {
                    rb.velocity = playerRelativePosition.normalized * speed;
                }
                else // less accurate, less predictable
                {
                    Vector2 direction = new Vector2(Mathf.Cos(((rb.rotation + 90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation + 90) * Mathf.PI) / 180));
                    rb.velocity = direction.normalized * speed;
                }
            }
            else
            {
                speed = 0;
                rb.velocity =Vector2.zero;
            }

            if (enhancedTargeting)  // much more accurate, also more predictable
            {
                rb.rotation = playerDirection + 90;
            }
            else // less accurate, less predictable
            {
                if (playerDirection > (180 + rb.rotation+90)%360 + turnSpeed)
                {
                    if ( 180 > (180 + rb.rotation + 90 + turnSpeed) % 360 - playerDirection)
                    {
                        rb.angularVelocity += turnSpeed;
                        //rb.rotation += turnSpeed;
                    }
                    else
                    {
                        rb.angularVelocity -= turnSpeed;
                    }
                }
                else if (playerDirection < (180 + rb.rotation + 90) % 360 - turnSpeed) {
                    if (180 < (180 + rb.rotation + 90 + turnSpeed) % 360 - playerDirection)
                    {
                        rb.angularVelocity += turnSpeed;
                    }
                    else
                    {
                        rb.angularVelocity -= turnSpeed;
                    }
                }
                else
                {
                    rb.angularVelocity = 0;// rb.angularVelocity * angularDrag;
                }
            }
        }
        else if (invasionEnemy)
        {
            if(speed < maxSpeed)
            {
                speed += thrust;
            }
            
            if(transform.position.magnitude >= distanceAwayToStop)
            {
                transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, speed * Time.deltaTime);
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                Home home = FindObjectOfType<Home>();
                if (home.numberOfHomePlanets > 0)
                {
                    int index = Random.Range(0, home.homePlanets.Count - 1);
                    if(home.homePlanets[index] != null)
                    {
                        target = home.homePlanets[index].gameObject;
                    }
                    else
                    {
                        while(home.homePlanets[index] == null)
                        {
                            index = Random.Range(0, home.homePlanets.Count - 1);
                        }
                        target = home.homePlanets[index].gameObject;
                    }
                    inTargetingRange = true;
                }
                else
                {
                    BroadcastMessage("You lose");
                }
            }

            Vector2 direction = Vector2.zero - (Vector2)transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + 180 + 90));
        }
        else
        {
            speed = 0;
            rb.velocity = new Vector2(0, 0);
            rb.angularVelocity = 0;
            if (boss && health < maxHealth)
            {
                health += maxHealth * .1f;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            if (!boss)
            {
                collision.gameObject.GetComponent<Player>().loseHealth(hitDamage);
                damage(maxHealth);
            }
            else
            {
                collision.gameObject.GetComponent<Player>().loseHealth(collision.gameObject.GetComponent<Player>().maxHealth);
            }
        }
        if (collision.transform.tag == "Friendly") // friendlies are just enemys in disguise
        {
            collision.gameObject.GetComponent<EnemyController>().damage(hitDamage);
            damage(maxHealth);
        }
        if (collision.transform.tag == "Planet")
        {
            if (!boss)
            {
                damage(maxHealth);
            }
            else
            {
                collision.gameObject.GetComponent<Planet>().destroy(true);
            }
        }
        if (collision.transform.tag == "Sun")
        {
            damage(maxHealth);
        }
    }
}
