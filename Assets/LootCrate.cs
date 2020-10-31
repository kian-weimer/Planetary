using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCrate : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public GameObject[] spawnItems;
    public int numberToSpawn;
    public GameObject deathExplosion;


    private void Start()
    {
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
        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject exp = Instantiate(spawnItems[Random.Range(0, spawnItems.Length)]);
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
