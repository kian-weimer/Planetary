using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;
    // Start is called before the first frame update
    public float despawnTime = 3;

    private float spawnTime;
    public float bulletDamage = 5;
    public bool extraDamageToPlanets = false;
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
            GameObject exp = Instantiate(explosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            if (extraDamageToPlanets)
            {
                collision.gameObject.GetComponent<Planet>().health = collision.gameObject.GetComponent<Planet>().health - bulletDamage * 1.5f;
            }
            else
            {
                collision.gameObject.GetComponent<Planet>().health = collision.gameObject.GetComponent<Planet>().health - bulletDamage;
            }

            if (collision.gameObject.GetComponent<Planet>().health <= 0)
            {
                if (!collision.gameObject.GetComponent<Planet>().destroyed)
                {
                    collision.gameObject.GetComponent<Planet>().destroyed = true;
                    collision.gameObject.GetComponent<Planet>().destroy(Player.doubleResource);
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
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<EnemyController>() != null)
            {
                collision.gameObject.GetComponent<EnemyController>().damage(bulletDamage);
            }
            else
            {
                collision.gameObject.GetComponent<LootCrate>().damage(bulletDamage);
            }

        }
        if(collision.gameObject.tag == "Mine")
        {
            collision.gameObject.GetComponent<Mine>().damageSurroundings();
        }

        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Popup" && collision.gameObject.tag != "HomeCircle" 
            && collision.gameObject.tag != "resource" && collision.gameObject.tag != "enemyRangeCollider"
            && collision.gameObject.tag != "playerRange" && collision.gameObject.tag != "Shield" && collision.gameObject.tag != "Bullet" && collision.gameObject.tag != "enemyBullet")
        {
            GameObject exp = Instantiate(explosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            Destroy(gameObject);
        }
    }
}