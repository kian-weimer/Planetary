using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject player;
    public Weapon lazerGun;
    public GameObject money;
    public Dictionary<string, int> resourceCost = new Dictionary<string, int>();

    void Start()
    {
        resourceCost.Add("Rock", 10);
        resourceCost.Add("Water", 15);
        resourceCost.Add("Coal", 15);
        resourceCost.Add("Diamond", 100);
        resourceCost.Add("Iron", 20);
        resourceCost.Add("Lava", 25);
        resourceCost.Add("Lead", 20);
        resourceCost.Add("Mercury", 25);
        resourceCost.Add("Obsidian", 30);
        resourceCost.Add("Oxygen", 15);
        resourceCost.Add("Wood", 15);
        resourceCost.Add("Satellite", 150);
        resourceCost.Add("Dark Matter", 200);
        resourceCost.Add("ahhh", 15);
    }
    public void buyShopResultOf(string shopItemName)
    {
        switch (shopItemName)
        {
            case "Armor Up":
                player.GetComponent<Player>().maxHealth += 50;
                break;

            //set up lazer gun image
            case "Lazers":
                player.GetComponent<Player>().weapon = lazerGun;
                break;

            case "Bigger Tank":
                player.GetComponent<Player>().maxGas += 50;
                break;

            case "Better Thrusters":
                player.GetComponent<PlayerController>().maxSpeed += 15;
                break;

            case "Warp Speed":
                player.GetComponent<PlayerController>().hasWarpSpeed = true;
                break;

            //update to addInventory Slot
            case "More Inventory":
                FindObjectOfType<Inventory>().addInventorySlot();
                break;

            case "Upgrade Ship":
                player.GetComponent<Player>().maxHealth += 100;
                player.GetComponent<Player>().maxGas += 100;
                player.GetComponent<PlayerController>().thrust += .3f;
                player.GetComponent<Player>().weapon.firerate += 2;
                player.GetComponent<Player>().weapon.bulletSpeed += 5;
                player.GetComponent<Player>().weapon.bullet.bulletDamage += 2;
                //update player icon to different ship
                //update bullet Damage
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
                    Destroy(playerItem.GetComponent<InventorySlot>().item);
                    Destroy(playerItem.GetComponent<InventorySlot>().icon);
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
                    Debug.Log(i);
                    if (planet.GetComponent<HomePlanet>().items[i].resource != null)
                    {
                        if (planet.GetComponent<HomePlanet>().items[i].resource.GetComponent<rsrce>().nameOfResource == Name)
                        {
                            if (planet.GetComponent<HomePlanet>().items[i].quantity > item.quantity)
                            {
                                Debug.Log(i + "  " + item.quantity);
                                planet.GetComponent<HomePlanet>().removeItem(i, item.quantity);
                                planet.GetComponent<HomePlanet>().UpdateUI();
                                numberDeleted = item.quantity;
                            }
                            else
                            {
                                planet.GetComponent<HomePlanet>().removeItem(i, planet.GetComponent<HomePlanet>().items[i].quantity);
                                planet.GetComponent<HomePlanet>().UpdateUI();
                                numberDeleted += planet.GetComponent<HomePlanet>().items[i].quantity;
                            }

                            if (numberDeleted == item.quantity)
                            {
                                return;
                            }
                        }
                    }

                }
            }
        }
    }
}
