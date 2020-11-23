using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public GameObject explosion;
    // Start is called before the first frame update
    public float despawnTime = 3;

    private float spawnTime;
    public float bulletDamage = 5;

    public bool trackingBullet = false;
    public bool targeting = false;
    public float trackingIntensity = 10f;


    Rigidbody2D rb;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
        if (trackingBullet)
        {
            rb = GetComponent<Rigidbody2D>();
            player = FindObjectOfType<Player>().gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= spawnTime + despawnTime)
        {
            GameObject exp = Instantiate(explosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Destroy(gameObject);
        }
    }

    private int trackCount = 0;
    private void FixedUpdate()
    {
        if (targeting && trackingBullet && trackCount < trackingIntensity)
        {
            Vector3 playerRelativePosition = player.transform.position - transform.position;
            rb.velocity = (Vector2)playerRelativePosition.normalized * trackingIntensity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            collision.gameObject.GetComponent<Planet>().health = collision.gameObject.GetComponent<Planet>().health - (int)bulletDamage;

            if (collision.gameObject.GetComponent<Planet>().health <= 0)
            {
                if (!collision.gameObject.GetComponent<Planet>().destroyed)
                {
                    collision.gameObject.GetComponent<Planet>().destroyed = true;
                    collision.gameObject.GetComponent<Planet>().destroy(false);
                }
            }
        }

        if (collision.gameObject.tag == "Satellite")
        {
            collision.gameObject.GetComponent<Satellite>().health = collision.gameObject.GetComponent<Satellite>().health - (int)bulletDamage;

            if (collision.gameObject.GetComponent<Satellite>().health <= 0)
            {
                if (!collision.gameObject.GetComponent<Satellite>().destroyed)
                {
                    collision.gameObject.GetComponent<Satellite>().destroyed = true;
                    collision.gameObject.GetComponent<Satellite>().destroy();
                }
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().loseHealth(bulletDamage);
        }
        if (collision.gameObject.tag == "Friendly") // friendlies are just enemys that cant move
        {
            collision.gameObject.GetComponent<EnemyController>().damage(bulletDamage);
        }

        if (collision.transform.CompareTag("playerRange"))
        {
            targeting = true;
        }
        if (targeting && trackingBullet)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * 0.50f;
        }

        if (collision.gameObject.tag == "Mine")
        {
            collision.gameObject.GetComponent<Mine>().damageSurroundings();
        }
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Popup" && collision.gameObject.tag != "HomeCircle"
            && collision.gameObject.tag != "resource" && collision.gameObject.tag != "enemyRangeCollider" && collision.gameObject.tag != "Trader"
             && collision.gameObject.tag != "playerRange" && collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "background")
        {
            GameObject exp = Instantiate(explosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            Destroy(gameObject);
        }

    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("playerRange"))
        {
            targeting = false;
        }
    }
}
