﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
    public static int maxHomePlanets = 20;

    public GameObject buyShop;
    public GameObject sellShop;
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
    public int amountOfWarps = 0;

    public Text warpAmount;
    public GameObject warpHolder;

    public GameObject drone;
    public GameObject turret;
    public GameObject decoy;
    public GameObject heal;
    public GameObject shield;

    private int numberOfCommon;
    private int numberOfRare;
    private int numberOfLegendary;

    public void Save()
    {
        PlayerPrefs.SetInt("whichShipColorBase", whichShipColorBase);
        PlayerPrefs.SetInt("whichShipLevel", whichShipLevel);
        if (isCheaper)
        {
            PlayerPrefs.GetInt("isCheeper", 1);
        }
        else
        {
            PlayerPrefs.GetInt("isCheeper", 0);
        }

        PlayerPrefs.SetInt("amountOfWarps", amountOfWarps);

        //saving the 2 shops
        FileStream fs = new FileStream("savedBuyShopItems.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, buyShop.GetComponent<Shop>().items);
        fs.Close();

        FileStream fs2 = new FileStream("savedSellShopItems.dat", FileMode.Create);
        BinaryFormatter bf2 = new BinaryFormatter();
        List<ShopItemInfo> shoppp = sellShop.GetComponent<Shop>().items;
        bf2.Serialize(fs2, shoppp);
        fs2.Close();

        PlayerPrefs.SetInt("numberOfCommon", numberOfCommon);
        PlayerPrefs.SetInt("numberOfRare", numberOfRare);
        PlayerPrefs.SetInt("numberOfLegendary", numberOfLegendary);
        PlayerPrefs.SetInt("amountOfWarps", amountOfWarps);
    }

    public void Load()
    {
        whichShipColorBase = PlayerPrefs.GetInt("whichShipColorBase");
        whichShipLevel = PlayerPrefs.GetInt("whichShipLevel");
        isCheaper = PlayerPrefs.GetInt("isCheeper") == 1;
        amountOfWarps = PlayerPrefs.GetInt("amountOfWarps");

        //shops
        using (Stream stream = File.Open("savedBuyShopItems.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            buyShop.GetComponent<Shop>().items.Clear();
            buyShop.GetComponent<Shop>().shopItems.Clear();
            buyShop.GetComponent<Shop>().itemsHudObjects.Clear();
            foreach (Transform itemToBeDeleted in buyShop.transform)
            {
                Destroy(itemToBeDeleted.gameObject);
            }
            buyShop.GetComponent<Shop>().items = (List<ShopItemInfo>)bformatter.Deserialize(stream);
        }

        buyShop.GetComponent<Shop>().loadUp();

        using (Stream stream = File.Open("savedSellShopItems.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            sellShop.GetComponent<Shop>().items.Clear();
            sellShop.GetComponent<Shop>().shopItems.Clear();
            sellShop.GetComponent<Shop>().itemsHudObjects.Clear();
            foreach (Transform itemToBeDeleted in sellShop.transform)
            {
                Destroy(itemToBeDeleted.gameObject);
            }
            sellShop.GetComponent<Shop>().items = (List<ShopItemInfo>)bformatter.Deserialize(stream);
        }

        sellShop.GetComponent<Shop>().loadUp();

        numberOfCommon = PlayerPrefs.GetInt("numberOfCommon");
        for (int i = 0; i < numberOfCommon; i++)
        {
            GameObject crate = Instantiate(commonCrate);
            crate.transform.position = crateSpawnPoint.transform.position + new Vector3(0, -10, 0);
            MaxItemsManager.addLootCrate();
        }

        numberOfRare = PlayerPrefs.GetInt("numberOfRare");
        for (int i = 0; i < numberOfRare; i++)
        {
            GameObject crateRare = Instantiate(rareCrate);
            crateRare.transform.position = crateSpawnPoint.transform.position + new Vector3(0, -10, 0);
            MaxItemsManager.addLootCrate();
        }

        numberOfLegendary = PlayerPrefs.GetInt("numberOfLegendary");
        for (int i = 0; i < numberOfLegendary; i++)
        {
            GameObject crateLegendary = Instantiate(legendaryCrate);
            crateLegendary.transform.position = crateSpawnPoint.transform.position + new Vector3(0, -10, 0);
            MaxItemsManager.addLootCrate();
        }
        amountOfWarps = PlayerPrefs.GetInt("amountOfWarps");
        if(amountOfWarps > 0)
        {
            warpHolder.SetActive(true);
            warpAmount.text = "x" + amountOfWarps.ToString();
        }
        
    }

    void Start()
    {
        loadDifficlutySettings(); // DIFFICULTY PARMS OVERWRITE THE INSPECTOR VALUES
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

        resourceCost.Add("FriendlySpawn", 0);

        resourceCost.Add("Copper", 50);
        resourceCost.Add("Ice", 50);
        resourceCost.Add("Plant", 50);
        resourceCost.Add("Sulfur", 50);
    }

    public void lowerAmountOfCrate(int rarity)
    {
        if(rarity == 1)
        {
            numberOfCommon -= 1;
        }
        if (rarity == 2)
        {
            numberOfRare-= 1;
        }
        if (rarity == 3)
        {
            numberOfLegendary -= 1;
        }
    }

    public void loadDifficlutySettings() // DIFFICULTY PARMS OVERWRITE THE INSPECTOR VALUES
    {
        if (PlayerPrefs.HasKey("upgradeAmount"))
        {
            upgradeAmount = PlayerPrefs.GetFloat("upgradeAmount");
        }
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
                player.GetComponent<PlayerController>().maxSpeed += 3;
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount * cheeperMultiplier));
                break;

            case "Add Rock Planet":
                home.GetComponent<Home>().addRockPlanet();
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount * .98f * cheeperMultiplier));

                MaxItemsManager.priceOfPlanet = (int)(shopItem.cost * upgradeAmount * .98f * cheeperMultiplier);
                if (home.GetComponent<Home>().numberOfHomePlanets >= maxHomePlanets)
                {
                    buyShop.GetComponent<Shop>().RemoveItem(shopItem.name);
                    MaxItemsManager.priceOfPlanet = shopItem.cost;
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

            case "Drone":
                GameObject droneItem = Instantiate(drone);
                droneItem.transform.position = player.transform.position;
                break;

            case "Turret":
                GameObject turretItem = Instantiate(turret);
                turretItem.transform.position = player.transform.position;
                break;

            case "Decoy":
                GameObject decoyItem = Instantiate(decoy);
                decoyItem.transform.position = player.transform.position;
                break;

            case "Heal":
                GameObject healItem = Instantiate(heal);
                healItem.transform.position = player.transform.position;
                break;

            case "Shield":
                GameObject shieldItem = Instantiate(shield);
                shieldItem.transform.position = player.transform.position;
                break;

            case "Buy Mine":
                MaxItemsManager.addMine();
                mineAmountText.GetComponent<Text>().text = "x" + MaxItemsManager.mineAmount;
                break;

            case "Common Crate":
                GameObject crate = Instantiate(commonCrate);
                crate.transform.position = crateSpawnPoint.transform.position + new Vector3(0, -10, 0);
                BM.Broadcast("Your purchased loot crate has been delivered near spawn!");
                MaxItemsManager.addLootCrate();
                numberOfCommon++;
                break;

            case "Rare Crate":
                GameObject crateRare = Instantiate(rareCrate);
                crateRare.transform.position = crateSpawnPoint.transform.position + new Vector3(0, -10, 0);
                BM.Broadcast("Your purchased loot crate has been delivered near spawn!");
                MaxItemsManager.addLootCrate();
                numberOfRare++;
                break;

            case "Legendary Crate":
                GameObject crateLegendary = Instantiate(legendaryCrate);
                crateLegendary.transform.position = crateSpawnPoint.transform.position + new Vector3(0, -10, 0);
                BM.Broadcast("Your purchased loot crate has been delivered near spawn!");
                MaxItemsManager.addLootCrate();
                numberOfLegendary++;
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

                player.GetComponent<Player>().gasBar.transform.localScale = new Vector3(1 * (player.GetComponent<Player>().gas - player.GetComponent<Player>().maxGas * .1f) / (player.GetComponent<Player>().maxGas - player.GetComponent<Player>().maxGas * .1f), player.GetComponent<Player>().gasBar.transform.localScale.y, player.GetComponent<Player>().gasBar.transform.localScale.z);

                if (player.GetComponent<Player>().gas <= player.GetComponent<Player>().maxGas * .1f)
                {
                    player.GetComponent<Player>().gasBarEnd.transform.localScale = new Vector3(-1 * (player.GetComponent<Player>().maxGas * .1f - player.GetComponent<Player>().gas) / (player.GetComponent<Player>().maxGas * .1f), player.GetComponent<Player>().gasBarEnd.transform.localScale.y, player.GetComponent<Player>().gasBarEnd.transform.localScale.z);
                }

                break;

            case "Random Recipe":

                List<PlanetCombo> randomList = comboList.planetComboList;
                
                int randomIndex = Random.Range(0, randomList.Count - 1);
                PlanetCombo combo = randomList[randomIndex];
                int startRandom = randomIndex;

                while (!alminac.AddEntry(combo.planet.gameObject.GetComponent<SpriteRenderer>().sprite, combo.item1.name, combo.item2.name, combo.item3.name) && !(randomIndex == startRandom - 1))
                {
                    randomIndex += 1;

                    
                    combo = randomList[randomIndex];

                    if(randomIndex == randomList.Count - 1)
                    {
                        randomIndex = -1;
                    }
                }
                
                if(randomIndex == startRandom - 1)
                {
                    FindObjectOfType<canvas>().friendlyBuyShop.RemoveItem("Random Recipe");
                    BM.Broadcast("You have bought all of the recipes");
                }
                
                break;
        }

        //buying resources
        if (listOfResources[0] != null && shopItem.name == listOfResources[0].nameOfResource)
        {
            GameObject resourceSpawn = Instantiate(listOfResources[0].gameObject);
            resourceSpawn.tag = "resource";
            resourceSpawn.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 velocityDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }

            resourceSpawn.GetComponent<Rigidbody2D>().velocity = velocityDirection;
            resourceSpawn.GetComponent<Rigidbody2D>().angularVelocity = 720;
        }
        else if (listOfResources[1] != null && shopItem.name == listOfResources[1].nameOfResource)
        {
            GameObject resourceSpawn = Instantiate(listOfResources[1].gameObject);
            resourceSpawn.tag = "resource";
            resourceSpawn.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 velocityDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            }

            resourceSpawn.GetComponent<Rigidbody2D>().velocity = velocityDirection;
            resourceSpawn.GetComponent<Rigidbody2D>().angularVelocity = 720;

        }
        else if (listOfResources[2] != null && shopItem.name == listOfResources[2].nameOfResource)
        {
            GameObject resourceSpawn = Instantiate(listOfResources[2].gameObject);
            resourceSpawn.tag = "resource";
            resourceSpawn.transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 velocityDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));

            while ((velocityDirection.x < .5f && velocityDirection.x > -.5f) && (velocityDirection.y < .5f && velocityDirection.y > -.5f))
            {
                velocityDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
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
