using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trader : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float maxTimeBeforeDeletion;
    public float timeTillDeletion;
    public GameObject explosion;
    private bool toldUserAlmostDone = false;

    public List<rsrce> listOfResources;
    private canvas canvasHolder;
    private ShopManager shopManager;
    public float additionalCostMultiplier;

    public List<GameObject> itemsTraderIsSelling;
    private bool drivingAway = false;
    public float distanceAway;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 playerPosition = FindObjectOfType<Player>().transform.position;

        float angle = Random.Range(0f, 360f);

        float xDirection = distanceAway * Mathf.Cos(angle);
        float yDirection = distanceAway * Mathf.Sin(angle);
        Vector2 offset = new Vector2(xDirection, yDirection);

        transform.position = playerPosition + offset;

        canvasHolder = FindObjectOfType<canvas>();
        shopManager = FindObjectOfType<ShopManager>();
        
        //3 random resources to buy
        int indexOfResource1 = Random.Range(0, listOfResources.Count - 1);
        int indexOfResource2 = Random.Range(0, listOfResources.Count - 1);
        int indexOfResource3 = Random.Range(0, listOfResources.Count - 1);

        while (listOfResources[indexOfResource1].nameOfResource == listOfResources[indexOfResource2].nameOfResource || listOfResources[indexOfResource1].nameOfResource == listOfResources[indexOfResource3].nameOfResource || listOfResources[indexOfResource2].nameOfResource == listOfResources[indexOfResource3].nameOfResource)
        {
            indexOfResource1 = Random.Range(0, listOfResources.Count - 1);
            indexOfResource2 = Random.Range(0, listOfResources.Count - 1);
            indexOfResource3 = Random.Range(0, listOfResources.Count - 1);
        }

        ShopItemInfo buyItem1 = new ShopItemInfo();
        buyItem1.name = listOfResources[indexOfResource1].nameOfResource;
        buyItem1.cost = (int)(shopManager.resourceCost[listOfResources[indexOfResource1].nameOfResource] * additionalCostMultiplier);
        canvasHolder.friendlyBuyShop.GetComponent<Shop>().addShopItem(buyItem1);
        shopManager.listOfResources[0] = listOfResources[indexOfResource1];

        ShopItemInfo buyItem2 = new ShopItemInfo();
        buyItem2.name = listOfResources[indexOfResource2].nameOfResource;
        buyItem2.cost = (int)(shopManager.resourceCost[listOfResources[indexOfResource1].nameOfResource] * additionalCostMultiplier);
        canvasHolder.friendlyBuyShop.GetComponent<Shop>().addShopItem(buyItem2);
        shopManager.listOfResources[1] = listOfResources[indexOfResource2];

        ShopItemInfo buyItem3 = new ShopItemInfo();
        buyItem3.name = listOfResources[indexOfResource3].nameOfResource;
        buyItem3.cost = (int)(shopManager.resourceCost[listOfResources[indexOfResource1].nameOfResource] * additionalCostMultiplier);
        canvasHolder.friendlyBuyShop.GetComponent<Shop>().addShopItem(buyItem3);
        shopManager.listOfResources[2] = listOfResources[indexOfResource3];

        //generate 3 random resources to sell
        
        indexOfResource1 = Random.Range(0, listOfResources.Count - 1);
        indexOfResource2 = Random.Range(0, listOfResources.Count - 1);
        indexOfResource3 = Random.Range(0, listOfResources.Count - 1);

        while(listOfResources[indexOfResource1].nameOfResource == listOfResources[indexOfResource2].nameOfResource || listOfResources[indexOfResource1].nameOfResource == listOfResources[indexOfResource3].nameOfResource || listOfResources[indexOfResource2].nameOfResource == listOfResources[indexOfResource3].nameOfResource)
        {
            indexOfResource1 = Random.Range(0, listOfResources.Count - 1);
            indexOfResource2 = Random.Range(0, listOfResources.Count - 1);
            indexOfResource3 = Random.Range(0, listOfResources.Count - 1);
        }

        ShopItemInfo sellItem1 = new ShopItemInfo();
        sellItem1.name = listOfResources[indexOfResource1].nameOfResource;

        sellItem1.cost = (int)(shopManager.resourceCost[listOfResources[indexOfResource1].nameOfResource] * additionalCostMultiplier);
        sellItem1.sellItem = true;
        canvasHolder.friendlySellShop.GetComponent<Shop>().addShopItem(sellItem1);
        itemsTraderIsSelling[0] = listOfResources[indexOfResource1].gameObject;

        ShopItemInfo sellItem2 = new ShopItemInfo();
        sellItem2.name = listOfResources[indexOfResource2].nameOfResource;
        sellItem2.cost = (int)(shopManager.resourceCost[listOfResources[indexOfResource2].nameOfResource] * additionalCostMultiplier);
        sellItem2.sellItem = true;
        canvasHolder.friendlySellShop.GetComponent<Shop>().addShopItem(sellItem2);
        itemsTraderIsSelling[1] = listOfResources[indexOfResource2].gameObject;

        ShopItemInfo sellItem3 = new ShopItemInfo();
        sellItem3.name = listOfResources[indexOfResource3].nameOfResource;
        sellItem3.cost = (int)(shopManager.resourceCost[listOfResources[indexOfResource3].nameOfResource] * additionalCostMultiplier);
        sellItem3.sellItem = true;
        canvasHolder.friendlySellShop.GetComponent<Shop>().addShopItem(sellItem3);
        itemsTraderIsSelling[2] = listOfResources[indexOfResource3].gameObject;

        health = maxHealth;
        timeTillDeletion = maxTimeBeforeDeletion;
    }

    // Update is called once per frame
    void Update()
    {
        timeTillDeletion -= Time.deltaTime;
        if(timeTillDeletion <= 30 && !toldUserAlmostDone)
        {
            FindObjectOfType<canvas>().broadcast("The trader is about to leave");
            toldUserAlmostDone = true;
        }

        if (timeTillDeletion <= 0 && !drivingAway)
        {
            canvasHolder.closeTrader();
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(.25f,0);
            drivingAway = true;
            FindObjectOfType<canvas>().broadcast("The trader is leaving");
        }
        if (drivingAway)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * 1.001f;
        }

        if(timeTillDeletion <= -20)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if(collision.gameObject.tag == "Bullet")
        {
            health -= collision.gameObject.GetComponent<Bullet>().bulletDamage;
            GameObject exp = Instantiate(collision.gameObject.GetComponent<Bullet>().explosion);
            exp.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z);
            FindObjectOfType<AudioManager>().Play("BulletExplosion");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Meteor")
        {
            health -= 5;
            collision.gameObject.GetComponent<Meteor>().destroy();
        }
        else if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().bounceBack();
            health -= 20;
        }

        if (health <= 0)
        {
            death();
        }
    }

    private void death()
    {
        GameObject exp = Instantiate(explosion);
        explosion.transform.position = gameObject.transform.position;
        canvasHolder.disableFriendlyTraderShopButton();

        GameObject resource = Instantiate(itemsTraderIsSelling[Random.Range(0, itemsTraderIsSelling.Count - 1)]);
        resource.tag = "resource";
        resource.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y );
        Vector2 velocityDirection = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f));

        while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
        {
            velocityDirection = new Vector2(Random.Range(-2.5f, 2.5f), Random.Range(-2.5f, 2.5f));
        }

        resource.GetComponent<Rigidbody2D>().velocity = velocityDirection;
        resource.GetComponent<Rigidbody2D>().angularVelocity = 720;

        canvasHolder.closeTrader();
        Destroy(gameObject);
    }
}
