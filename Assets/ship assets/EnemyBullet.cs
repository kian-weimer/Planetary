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
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= spawnTime + despawnTime)
        {
            Destroy(gameObject);
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
                    collision.gameObject.GetComponent<Planet>().destroy();
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

        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Popup" && collision.gameObject.tag != "HomeCircle"
            && collision.gameObject.tag != "resource" && collision.gameObject.tag != "enemyRangeCollider"
             && collision.gameObject.tag != "playerRange")
        {
            GameObject exp = Instantiate(explosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            Destroy(gameObject);
        }
    }
}
