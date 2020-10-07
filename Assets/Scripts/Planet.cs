using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


[System.Serializable]
public class Planet : MonoBehaviour
{
    public PlanetInfo info; // a Serializable list of the planet's properties
    public int rarity; // the rarity of the planet
    public int health; // planet's health
    public Vector3 position;
    //public int type; // the type of planet (used to create image)
    public bool discovered; // true is the planet has been seen
    public GameObject planetExplosion; // holds the prefab of the explosionm animation used when the planet is destroyed
    public GameObject planetResource; // holds the item resource that the planet pops out when destroyed
    public bool inHomeSystem = false; // is true when its in the home solar system

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

    }
    public void destroy()
    {
        GameObject exp = Instantiate(planetExplosion);
        exp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        
        GameObject resource = Instantiate(planetResource);
        planetResource.tag = "resource";
        resource.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        Vector2 velocityDirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));

        while((velocityDirection.x < .25f && velocityDirection.x > -.25f) && (velocityDirection.y < .25f && velocityDirection.y > -.25f))
        {
            velocityDirection = new Vector2(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        }

        resource.GetComponent<Rigidbody2D>().velocity = velocityDirection;
        resource.GetComponent<Rigidbody2D>().angularVelocity = 360;
        
        FindObjectOfType<planetGenerator>().destroyPlanet(this);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().health = collision.gameObject.GetComponent<Player>().health - 10;
            collision.gameObject.GetComponent<PlayerController>().bounceBack();
        }
    }
}
