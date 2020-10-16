using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject player;
    public Weapon lazerGun;
    public GameObject money;
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
                money.GetComponent<Money>().addMoney(item.cost);
                break;
        }
    }
}
