using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;
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
    private int whichShipColorBase = 0;
    private int whichShipLevel = 0;
    public GameObject buyShop;
    public float upgradeAmount;
    public GameObject boostBar;

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
        resourceCost.Add("Bullet", 20);
        resourceCost.Add("Steel", 30);
    }
    public void buyShopResultOf(ShopItem shopItem)
    {
        switch (shopItem.name)
        {
            case "Armor Up":
                player.GetComponent<Player>().maxHealth += 50;
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount));
                break;

            //set up lazer gun image
            case "Lazers":
                player.GetComponent<Player>().weapon = lazerGun;
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount));
                break;

            case "Bigger Tank":
                player.GetComponent<Player>().maxGas += 50;
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount));
                break;

            case "Better Thrusters":
                player.GetComponent<PlayerController>().maxSpeed += 5;
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount));
                break;

            case "Warp Speed":
                player.GetComponent<PlayerController>().hasWarpSpeed = true;
                boostBar.SetActive(true);
                buyShop.GetComponent<Shop>().RemoveItem(shopItem.name);
                break;

            //update to addInventory Slot
            case "More Inventory":
                FindObjectOfType<Inventory>().addInventorySlot();
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount));
                break;

            case "Upgrade Ship":
                player.GetComponent<Player>().maxHealth += 100;
                player.GetComponent<Player>().maxGas += 100;
                player.GetComponent<PlayerController>().maxSpeed += 2;
                player.GetComponent<PlayerController>().thrust += .3f;
                player.GetComponent<Player>().weapon.firerate += 2;
                player.GetComponent<Player>().weapon.bulletSpeed += 5;
                player.GetComponent<Player>().weapon.bullet.bulletDamage += 3;
                player.GetComponent<SpriteRenderer>().sprite = playerSprites[whichShipColorBase + whichShipLevel + 1];
                whichShipLevel += 1;
                if(whichShipLevel == 1)
                {
                    player.GetComponent<BoxCollider2D>().size = new Vector2(.7f, 1f);
                }
                
                if(whichShipLevel == 2)
                {
                    buyShop.GetComponent<Shop>().RemoveItem(shopItem.name);
                    player.GetComponent<BoxCollider2D>().size = new Vector2(.7f, 1.25f);
                }
                buyShop.GetComponent<Shop>().changePrice(shopItem.name, (int)(shopItem.cost * upgradeAmount));
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
        }
    }
    public void sellShopResultOf(SellShopItem item)
    {
        switch (item.name)
        {
            case "Rock":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Rock", item.quantity))
                {
                    findAndSellShopItem(item, "Rock");
                }
                break;

            case "Water":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Water", item.quantity))
                {
                    findAndSellShopItem(item, "Water");
                }
                break;
            case "Coal":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Coal", item.quantity))
                {
                    findAndSellShopItem(item, "Coal");
                }
                break;
            case "Dark Matter":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Dark Matter", item.quantity))
                {
                    findAndSellShopItem(item, "Dark Matter");
                }
                break;

            case "Diamond":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Diamond", item.quantity))
                {
                    findAndSellShopItem(item, "Diamond");
                }
                break;
            case "Iron":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Iron", item.quantity))
                {
                    findAndSellShopItem(item, "Iron");
                }
                break;
            case "Lava":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Lava", item.quantity))
                {
                    findAndSellShopItem(item, "Lava");
                }
                break;
            case "Lead":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Lead", item.quantity))
                {
                    findAndSellShopItem(item, "Lead");
                }
                break;
            case "Mercury":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Mercury", item.quantity))
                {
                    findAndSellShopItem(item, "Mercury");
                }
                break;
            case "Obsidian":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Obsidian", item.quantity))
                {
                    findAndSellShopItem(item, "Obsidian");
                }
                break;
            case "Oxygen":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Oxygen", item.quantity))
                {
                    findAndSellShopItem(item, "Oxygen");
                }
                break;
            case "Satellite":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Satellite", item.quantity))
                {
                    findAndSellShopItem(item, "Satellite");
                }
                break;
            case "Wood":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Wood", item.quantity))
                {
                    findAndSellShopItem(item, "Wood");
                }
                break;

            case "Food":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Food", item.quantity))
                {
                    findAndSellShopItem(item, "Food");
                }
                break;
            case "Gold":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Gold", item.quantity))
                {
                    findAndSellShopItem(item, "Gold");
                }
                break;
            case "Steel":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Steel", item.quantity))
                {
                    findAndSellShopItem(item, "Steel");
                }
                break;
            case "Poison Gas":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Poison Gas", item.quantity))
                {
                    findAndSellShopItem(item, "Poison Gas");
                }
                break;
            case "Bullet":
                if (FindObjectOfType<ResourceInventory>().checkForItemAndRemove("Bullet", item.quantity))
                {
                    findAndSellShopItem(item, "Bullet");
                }
                break;
        }
    }

    private void findAndSellShopItem(SellShopItem item, string Name)
    {
        money.GetComponent<Money>().addMoney(item.cost * item.quantity);
        int numberDeleted = 0;
        List<GameObject> playerInventory = FindObjectOfType<Inventory>().slots;
        foreach (GameObject playerItem in playerInventory)
        {
           
            if (playerItem.GetComponent<InventorySlot>().item != null)
            {

                if (playerItem.GetComponent<InventorySlot>().item.GetComponent<rsrce>().nameOfResource == Name)
                {
                    Destroy(playerItem.GetComponent<InventorySlot>().item); // BUG!!! when SELLING "DESTROYING ASSETS IS NOT PERMITTED TO AVOID DATA LOSS"
                    Destroy(playerItem.GetComponent<InventorySlot>().icon);
                    FindObjectOfType<Inventory>().isFull = false;

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
                                planet.GetComponent<HomePlanet>().removeItem(i, planet.GetComponent<HomePlanet>().items[i].quantity);
                                numberDeleted += planet.GetComponent<HomePlanet>().items[i].quantity;
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
