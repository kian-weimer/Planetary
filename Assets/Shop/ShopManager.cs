using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject player;
    public Weapon lazerGun;
    public GameObject money;
    public Dictionary<string, int> resourceCost = new Dictionary<string, int>();
    public GameObject home;
    public List<Sprite> playerSprites;
    [HideInInspector]
    public int whichShipColorBase = 0;
    public int whichShipLevel = 0;
    public int maxHomePlanets = 20;

    public GameObject buyShop;
    public float upgradeAmount;
    public float expCheeperShopMultiplier = 1;

    public static bool isCheaper = false;
    public GameObject mineAmountText;

    public GameObject commonCrate;
    public GameObject rareCrate;
    public GameObject legendaryCrate;
    public GameObject crateSpawnPoint;
    public BroadcastMessage BM;
    public List<rsrce> listOfResources;
    public Alminac alminac;
    public PlanetComboList comboList;
    public int amountOfWarps;

    public Text warpAmount;
    public GameObject warpHolder;

    void Start()
    {
        resourceCost.Add("Rock", 1);
        resourceCost.Add("Water", 15);
        resourceCost.Add("Coal", 20);
        resourceCost.Add("Diamond", 100);
        resourceCost.Add("Iron", 25);
        resourceCost.Add("Lava", 25);
        resourceCost.Add("Lead", 25);
        resourceCost.Add("Mercury", 25);
        resourceCost.Add("Obsidian", 40);
        resourceCost.Add("Oxygen", 10);
        resourceCost.Add("Wood", 15);
        resourceCost.Add("Satellite", 150);
        resourceCost.Add("Dark Matter", 750);
        resourceCost.Add("Food", 25);
        resourceCost.Add("Gold", 75);
        resourceCost.Add("Poison Gas", 5);
        resourceCost.Add("Armory", 125);
        resourceCost.Add("Steel", 30);
        resourceCost.Add("EndGame", 10000);

        resourceCost.Add("Copper", 50);
        resourceCost.Add("Ice", 50);
        resourceCost.Add("Plant", 50);
        resourceCost.Add("Sulfur", 50);
    }
    public void buyShopResultOf(ShopItem shopItem)
    {
        float cheeperMultiplier = 1f;
        if (isCheaper)
        {
            cheeperMultiplier = expCheeperShopMultiplier;
        }
        switch (shopItem.name)
        {
            case "Armor Up":
                player.GetComponent<Player>().maxHealth += 50;
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount * cheeperMultiplier));
                break;

            //set up lazer gun image
            case "Lazers":
                player.GetComponent<Player>().weapon = lazerGun.gameObject;
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount * cheeperMultiplier));
                break;

            case "Bigger Tank":
                player.GetComponent<Player>().maxGas += 50;
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount * cheeperMultiplier));
                break;

            case "Better Thrusters":
                player.GetComponent<PlayerController>().maxSpeed += 5;
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount * cheeperMultiplier));
                break;

            case "Add Rock Planet":
                home.GetComponent<Home>().addRockPlanet();
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount * cheeperMultiplier));
                if (home.GetComponent<Home>().numberOfStartingHomePlanets >= maxHomePlanets)
                {
                    buyShop.GetComponent<Shop>().RemoveItem(shopItem.name);
                }
                break;

            //update to addInventory Slot
            case "More Inventory":
                FindObjectOfType<Inventory>().addInventorySlot();
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount * cheeperMultiplier));
                if (FindObjectOfType<Inventory>().numberOfInventorySlots == 8)
                {
                    buyShop.GetComponent<Shop>().RemoveItem(shopItem.name);
                }
                break;

            case "Switch Color":
                if(whichShipColorBase == playerSprites.Count - 3)
                {
                    player.GetComponent<SpriteRenderer>().sprite = playerSprites[whichShipLevel];
                    whichShipColorBase = 0;
                }
                else 
                {
                    player.GetComponent<SpriteRenderer>().sprite = playerSprites[whichShipColorBase + 3 + whichShipLevel];
                    whichShipColorBase += 3;
                }
                break;

            case "Buy Mine":
                MineControler.mineAmount += 1;
                mineAmountText.GetComponent<Text>().text = "x" + MineControler.mineAmount;
                break;

            case "Common Crate":
                GameObject crate = Instantiate(commonCrate);
                crate.transform.position = crateSpawnPoint.transform.position + new Vector3(0, -10, 0);
                BM.Broadcast("Your purchased loot crate has been delivered near spawn!");
                break;

            case "Rare Crate":
                GameObject crateRare = Instantiate(rareCrate);
                crateRare.transform.position = crateSpawnPoint.transform.position + new Vector3(0, -10, 0);
                BM.Broadcast("Your purchased loot crate has been delivered near spawn!");
                break;

            case "Legendary Crate":
                GameObject crateLegendary = Instantiate(legendaryCrate);
                crateLegendary.transform.position = crateSpawnPoint.transform.position + new Vector3(0, -10, 0);
                BM.Broadcast("Your purchased loot crate has been delivered near spawn!");
                break;

            //friendly trader shop
            case "Warp Home Tokens":

                if(amountOfWarps == 0)
                {
                    warpHolder.SetActive(true);
                    BM.Broadcast("You can press q to be teleported to home");
                }
                amountOfWarps += 1;
                warpAmount.text = "x" + amountOfWarps.ToString();

                break;

            case "Buy Gas":
                player.GetComponent<Player>().gas += player.GetComponent<Player>().maxGas * .1f;
                if(player.GetComponent<Player>().gas > player.GetComponent<Player>().maxGas)
                {
                    player.GetComponent<Player>().gas = player.GetComponent<Player>().maxGas;
                }

                player.GetComponent<Player>().gasBar.transform.localScale = new Vector3(0, player.GetComponent<Player>().gasBar.transform.localScale.y, player.GetComponent<Player>().gasBar.transform.localScale.z);
                player.GetComponent<Player>().gasBarEnd.transform.localScale = new Vector3(-1, player.GetComponent<Player>().gasBarEnd.transform.localScale.y, player.GetComponent<Player>().gasBarEnd.transform.localScale.z);
                break;

            case "Random Recipe":
                int randomIndex = Random.Range(0,comboList.planetComboList.Count - 1);
                PlanetCombo combo = comboList.planetComboList[randomIndex];
                alminac.AddEntry(combo.planet.gameObject.GetComponent<SpriteRenderer>().sprite, combo.item1.name, combo.item2.name, combo.item3.name);
                break;
        }

        //buying resources
        Debug.Log(shopItem.name + ", " + listOfResources[0].nameOfResource + ", " + listOfResources[1].nameOfResource + ", " + listOfResources[2].nameOfResource);
        if (shopItem.name == listOfResources[0].nameOfResource)
        {
            GameObject resourceSpawn = Instantiate(listOfResources[0].gameObject);
            resourceSpawn.tag = "resource";
            resourceSpawn.transform.position = new Vector2(player.transform.position.x + Random.Range(-5.5f, 5.5f), player.transform.position.y + Random.Range(-5.5f, 5.5f));
            Vector2 velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            }

            resourceSpawn.GetComponent<Rigidbody2D>().velocity = velocityDirection;
            resourceSpawn.GetComponent<Rigidbody2D>().angularVelocity = 720;
        }
        else if (shopItem.name == listOfResources[1].nameOfResource)
        {
            GameObject resourceSpawn = Instantiate(listOfResources[2].gameObject);
            resourceSpawn.tag = "resource";
            resourceSpawn.transform.position = new Vector2(player.transform.position.x + Random.Range(-5.5f, 5.5f), player.transform.position.y + Random.Range(-5.5f, 5.5f));
            Vector2 velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            }

            resourceSpawn.GetComponent<Rigidbody2D>().velocity = velocityDirection;
            resourceSpawn.GetComponent<Rigidbody2D>().angularVelocity = 720;

        }
        else if (shopItem.name == listOfResources[2].nameOfResource)
        {
            GameObject resourceSpawn = Instantiate(listOfResources[2].gameObject);
            resourceSpawn.tag = "resource";
            resourceSpawn.transform.position = new Vector2(player.transform.position.x + Random.Range(-5.5f, 5.5f), player.transform.position.y + Random.Range(-5.5f, 5.5f));
            Vector2 velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-2, 2), Random.Range(-2, 2));
            }

            resourceSpawn.GetComponent<Rigidbody2D>().velocity = velocityDirection;
            resourceSpawn.GetComponent<Rigidbody2D>().angularVelocity = 720;
        }

    }

    public void sellShopResultOf(SellShopItem item, bool isTrader)
    {
        if(FindObjectOfType<ResourceInventory>().checkForItemAndRemove(item.name, item.quantity) && !isTrader)
        {
            findAndSellShopItem(item, item.name);
            return;
        }

        //if friendly trader
        List<GameObject> playerInventory = FindObjectOfType<Inventory>().slots;
        int numberFound = 0;
        foreach (GameObject playerInventorySlot in playerInventory)
        {

            if (playerInventorySlot.GetComponent<InventorySlot>().item != null)
            {

                if (playerInventorySlot.GetComponent<InventorySlot>().item.GetComponent<rsrce>().nameOfResource == item.name)
                {
                    numberFound++;
                }
            }
        }

        if(numberFound >= item.quantity)
        {
            int numberDeleted = 0;
            foreach (GameObject playerInventorySlot in playerInventory)
            {

                if (playerInventorySlot.GetComponent<InventorySlot>().item != null)
                {

                    if (playerInventorySlot.GetComponent<InventorySlot>().item.GetComponent<rsrce>().nameOfResource == item.name)
                    {
                        var child = playerInventorySlot.transform.GetChild(0);
                        child.GetComponent<DragDrop>().destroy();
                        playerInventorySlot.GetComponent<InventorySlot>().RemoveItem();

                        numberDeleted++;
                        if (numberDeleted == item.quantity)
                        {
                            return;
                        }
                    }
                }
            }
        }


    }

    private void findAndSellShopItem(SellShopItem item, string Name)
    {
        money.GetComponent<Money>().addMoney(item.cost * item.quantity);
        int numberDeleted = 0;
        List<GameObject> playerInventory = FindObjectOfType<Inventory>().slots;
        foreach (GameObject playerInventorySlot in playerInventory)
        {
           
            if (playerInventorySlot.GetComponent<InventorySlot>().item != null)
            {

                if (playerInventorySlot.GetComponent<InventorySlot>().item.GetComponent<rsrce>().nameOfResource == Name)
                {
                    var child = playerInventorySlot.transform.GetChild(0);
                    child.GetComponent<DragDrop>().destroy();
                    playerInventorySlot.GetComponent<InventorySlot>().RemoveItem();

                    numberDeleted++;
                    if (numberDeleted == item.quantity)
                    {
                        return;
                    }
                }
            }
        }

        List<Planet> homePlanets = FindObjectOfType<Home>().homePlanets;
        
        foreach (Planet planet in homePlanets)
        {
            if (planet != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (planet.GetComponent<HomePlanet>().items[i].resource != null)
                    {
                        if (planet.GetComponent<HomePlanet>().items[i].resource.GetComponent<rsrce>().nameOfResource == Name)
                        {
                            if (planet.GetComponent<HomePlanet>().items[i].quantity > (item.quantity - numberDeleted))
                            {
                                planet.GetComponent<HomePlanet>().removeItem(i, item.quantity - numberDeleted);
                                numberDeleted += item.quantity;
                            }
                            else
                            {
                                numberDeleted += planet.GetComponent<HomePlanet>().items[i].quantity;
                                planet.GetComponent<HomePlanet>().removeItem(i, planet.GetComponent<HomePlanet>().items[i].quantity);
                            }

                            if (numberDeleted == item.quantity)
                            {
                                return;
                            }
                            home.GetComponent<Home>().UpdatePlanetHud();
                        }
                    }

                }
            }
        }
    }
}
