using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;
    public GameObject enemy;

    // Start is called before the first frame update
    public float despawnTime = 3;

    private float spawnTime;
    public float bulletDamage = 5;
    public bool extraDamageToPlanets = false;

    public bool trackingBullet = false;
    public bool targeting = false;
    public float trackingIntensity = 10f;
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

    private int trackCount = 0;
    private void FixedUpdate()
    {
        if (targeting && trackingBullet && trackCount < trackingIntensity)
        {
            try
            {
                Vector3 playerRelativePosition = enemy.transform.position - transform.position;
                GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity*0.99f + (Vector2)playerRelativePosition.normalized * trackingIntensity;
                trackCount++;
            }
            catch
            {
                targeting = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Planet")
        {
            try
            {
                FindObjectOfType<AudioManager>().Play("BulletExplosion");
                if (extraDamageToPlanets)
                {
                    collision.gameObject.GetComponent<Planet>().health = collision.gameObject.GetComponent<Planet>().health - bulletDamage * 1.5f;
                }
                else
                {
                    collision.gameObject.GetComponent<Planet>().health = collision.gameObject.GetComponent<Planet>().health - bulletDamage;
                }
            }
            catch (NullReferenceException e)
            {
                Debug.LogWarning("bulletHitIssueWithPlanetCaught");
            }
            
        }
        
        if (collision.transform.CompareTag("enemyRangeCollider") && collision.name == "StoppingRange")
        {
            targeting = true;
            enemy = collision.gameObject;
        }
        
        if (targeting && trackingBullet)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * 0.75f;
        }

        if (collision.gameObject.tag == "Satellite")
        {
            FindObjectOfType<AudioManager>().Play("BulletExplosion");
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

        if (collision.transform.CompareTag("PlanetShield"))
        {
            collision.GetComponent<PlanetShield>().damage(bulletDamage);
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
            FindObjectOfType<AudioManager>().Play("BulletExplosion");
        }
        if(collision.gameObject.tag == "Mine")
        {
            collision.gameObject.GetComponent<Mine>().damageSurroundings();
            FindObjectOfType<AudioManager>().Play("BulletExplosion");
        }

        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Popup" && collision.gameObject.tag != "HomeCircle" 
            && collision.gameObject.tag != "resource" && collision.gameObject.tag != "enemyRangeCollider"
            && collision.gameObject.tag != "playerRange" && collision.gameObject.tag != "Shield" && collision.gameObject.tag != "Bullet" 
            && collision.gameObject.tag != "enemyBullet" && collision.gameObject.tag != "background" && collision.gameObject.tag != "Trader" 
            && collision.gameObject.tag != "Friendly" && collision.gameObject.tag != "Exp")
        {
            if (collision.gameObject.GetComponent<Flash>() != null)
            {
                collision.gameObject.GetComponent<Flash>().start();
            }
            GameObject exp = Instantiate(explosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            FindObjectOfType<AudioManager>().Play("BulletExplosion");
            Destroy(gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("enemyRangeCollider") && collision.name == "StoppingRange")
        {
            targeting = false;
        }
    }
}