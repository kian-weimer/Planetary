
using System.Diagnostics.Eventing.Reader;
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


    private void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
        audioManager = FindObjectOfType<AudioManager>();
        rb = GetComponent<Rigidbody2D>();
        weapon = Instantiate(weapon);
        weapon.transform.parent = transform;
        health = maxHealth;
    }

    public void damage(float hitPoints)
    {
        health -= hitPoints;
        if (health <= 0)
        {
            GameObject exp = Instantiate(deathExplosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            spawnExp();
            Destroy(gameObject);
        }
    }

    public void spawnExp()
    {
        for (int i = 0; i < expPointsDropped; i++)
        {
            GameObject exp = Instantiate(expPoint);
            exp.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            Vector2 velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
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
            Vector3 playerRelativePosition = player.transform.position - transform.position;
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
                rb.velocity *= 0.90f;
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
        else
        {
            speed = 0;
            rb.velocity = new Vector2(0, 0);
            rb.angularVelocity = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().loseHealth(hitDamage);
            damage(maxHealth);
        }
        if (collision.transform.tag == "Planet")
        {
            damage(maxHealth);
        }
    }
}
