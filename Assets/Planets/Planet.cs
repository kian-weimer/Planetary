using System.Collections;
using System.Collections.Generic;
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
    public GameObject planetExplosion;

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
        Destroy(gameObject);
    }
}
