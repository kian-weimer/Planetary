using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[System.Serializable]
public class Planet : MonoBehaviour
{
    public PlanetInfo info; // a Serializable list of the planet's properties
    public int rarity; // the rarity of the planet
    public int maxHealth;
    public int health; // planet's health
    public Vector3 position;
    //public int type; // the type of planet (used to create image)
    public bool discovered; // true is the planet has been seen
    public GameObject planetExplosion; // holds the prefab of the explosionm animation used when the planet is destroyed
    public GameObject planetResource; // holds the item resource that the planet pops out when destroyed
    public bool inHomeSystem = false; // is true when its in the home solar system
    public bool destroyed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(PlanetInfo info)
    {
        this.info = info;
        position = new Vector3(info.position[0], info.position[1], 0);
        gameObject.transform.position = position;
        health = info.health;
        rarity = info.rarity;
        discovered = info.discovered;
        maxHealth = info.maxHealth;
    }
    public void destroy()
    {
        if (inHomeSystem)
        {

            // do something to show everything that it is gone (messes up the UI) 
            FindObjectOfType<Player>().HomePlanetDestroyed(this);
            Debug.Log(FindObjectOfType<Home>().homePlanets.IndexOf(this));
            FindObjectOfType<Home>().homePlanets[FindObjectOfType<Home>().homePlanets.IndexOf(this)] = null;
        }

        GameObject exp = Instantiate(planetExplosion);
        exp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        GameObject resource = Instantiate(planetResource);
        planetResource.tag = "resource";
        resource.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Vector2 velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

        while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
        {
            velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
        }

        resource.GetComponent<Rigidbody2D>().velocity = velocityDirection;
        resource.GetComponent<Rigidbody2D>().angularVelocity = 720;

        FindObjectOfType<planetGenerator>().destroyPlanet(this);


    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().loseHealth(40);
            collision.gameObject.GetComponent<PlayerController>().bounceBack();
        }
    }
}