using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int spawnDistance;
    public int damage;

    public int numberToSpawn;

    public GameObject explosion; // holds the prefab of the explosionm animation used when the planet is destroyed
    public GameObject resourceSpawn; // holds the item resource that the planet pops out when destroyed
    public bool destroyed = false;

    public void Start()
    {
        Vector3 position;
        health = maxHealth;
        int degree = (int)(Random.Range(0, 360) * Mathf.PI) / 180;
        position = (new Vector3(Mathf.Cos(degree), Mathf.Sin(degree), 0)) * spawnDistance + FindObjectOfType<Player>().transform.position;
        transform.position = position;
    }

    // Start is called before the first frame update
    public void destroy()
    {
        GameObject exp = Instantiate(explosion);
        exp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        for (int i = 0; i < numberToSpawn; i++)
        {
            GameObject resource = Instantiate(resourceSpawn);
            resourceSpawn.tag = "resource";
            resource.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            Vector2 velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            }

            resource.GetComponent<Rigidbody2D>().velocity = velocityDirection;
            resource.GetComponent<Rigidbody2D>().angularVelocity = 720;
        }
        Destroy(gameObject);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().loseHealth(damage);
            collision.gameObject.GetComponent<PlayerController>().bounceBack();
        }
    }
}
