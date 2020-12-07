using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Planet : MonoBehaviour
{
    public PlanetInfo info; // a Serializable list of the planet's properties
    public int rarity; // the rarity of the planet
    public float maxHealth;
    public float health; // planet's health
    public Vector3 position;
    //public int type; // the type of planet (used to create image)
    public int discovered; // 0 corrresponds to non discovered, 1 means discovered but the rarity is not known to the map, 2 means that rarity is known to the map// true is the planet has been seen
    public GameObject planetExplosion; // holds the prefab of the explosionm animation used when the planet is destroyed
    public GameObject planetResource; // holds the item resource that the planet pops out when destroyed
    public bool inHomeSystem = false; // is true when its in the home solar system
    public bool destroyed = false;
    private bool destroyedByMine = false;

    public void Initialize(PlanetInfo info)
    {
        this.info = info;
        position = new Vector3(info.position[0], info.position[1], 0);
        gameObject.transform.position = position;
        health = info.health;
        rarity = info.rarity;
        discovered = 1;
        maxHealth = info.maxHealth;
    }

    void Start()
    {
        if (GameManager.lightsOn)
        {
            GetComponent<Light2D>().enabled = true;
        }
    }

    public void destroy(bool doubleResource)
    {
        destroyedByMine = true;

        int planetExplosionNumber = Random.Range(1, 3);

        FindObjectOfType<AudioManager>().PlayIfNotPlaying("PlanetExplosion" + planetExplosionNumber);
        if (inHomeSystem)
        {

            // do something to show everything that it is gone (messes up the UI) 
            FindObjectOfType<Player>().HomePlanetDestroyed(this);
            FindObjectOfType<Home>().homePlanets[FindObjectOfType<Home>().homePlanets.IndexOf(this)] = null;
            FindObjectOfType<Home>().planetInfo[FindObjectOfType<Home>().planetInfo.IndexOf(this.info)] = null;


            float cheeperMultiplier = 1f;
            if (ShopManager.isCheaper)
            {
                cheeperMultiplier = FindObjectOfType<ShopManager>().expCheeperShopMultiplier;
            }

            
            if (FindObjectOfType<Home>().numberOfHomePlanets == ShopManager.maxHomePlanets)
            {
                ShopItemInfo shopItem = new ShopItemInfo();
                shopItem.name = "Add Rock Planet";
                shopItem.cost = (int)MaxItemsManager.priceOfPlanet;
                FindObjectOfType<canvas>().mainBuyShop.addShopItem(shopItem);
            }
            


            FindObjectOfType<canvas>().mainBuyShop.GetComponent<Shop>().changePrice("Add Rock Planet", (int)(MaxItemsManager.priceOfPlanet / FindObjectOfType<ShopManager>().upgradeAmount / .98f / cheeperMultiplier));
            //FindObjectOfType<Home>().numberOfHomePlanets--;
        }

        GameObject exp = Instantiate(planetExplosion);
        exp.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        int doubleChance = Random.Range(0, 100);

        if (doubleChance >= 50 && doubleResource)
        {
            GameObject resource = Instantiate(planetResource);
            planetResource.tag = "resource";
            resource.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            Vector2 velocityDirection = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f));
            }

            resource.GetComponent<Rigidbody2D>().velocity = velocityDirection;
            resource.GetComponent<Rigidbody2D>().angularVelocity = 720;

            GameObject resource2 = Instantiate(planetResource);
            planetResource.tag = "resource";
            resource2.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
            Vector2 velocityDirection2 = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

            while ((velocityDirection2.x < .5f && velocityDirection2.x > -.5f) && (velocityDirection2.y < .5f && velocityDirection2.y > -.5f))
            {
                velocityDirection2 = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            }

            resource2.GetComponent<Rigidbody2D>().velocity = velocityDirection2;
            resource2.GetComponent<Rigidbody2D>().angularVelocity = 720;

        }
        else
        {
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
        }

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

    public bool isDestroyedByMine()
    {
        return destroyedByMine;
    }

    private void Update()
    {
        // not a great solution, but prevents from editing every location that damages a planet...
        if (info.health != health)
        {
            info.health = health;
        }
    }
}