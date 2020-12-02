using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxItemsManager : MonoBehaviour
{
    public static int NumberOfLootCrates = 0;
    public static int MaxNumberOfLootCrates = 5;

    public static int maxMines = 10;
    public static int mineAmount = 0;

    public static float priceOfPlanet = 800;
    
    public static void addLootCrate()
    {
        if(NumberOfLootCrates + 1 == MaxNumberOfLootCrates)
        {
            NumberOfLootCrates = MaxNumberOfLootCrates;
            FindObjectOfType<canvas>().mainBuyShop.RemoveItem("Legendary Crate");
            FindObjectOfType<canvas>().mainBuyShop.RemoveItem("Rare Crate");
            FindObjectOfType<canvas>().mainBuyShop.RemoveItem("Common Crate");
        }
        else
        {
            NumberOfLootCrates += 1;
        }
    }

    public static void destroyLootCrate(int rarity)
    {
        if(NumberOfLootCrates == MaxNumberOfLootCrates)
        {
            ShopItemInfo shopItem = new ShopItemInfo();
            shopItem.name = "Common Crate";
            shopItem.cost = 1000;
            FindObjectOfType<canvas>().mainBuyShop.addShopItem(shopItem);

            ShopItemInfo shopItem2 = new ShopItemInfo();
            shopItem.name = "Rare Crate";
            shopItem.cost = 2000;
            FindObjectOfType<canvas>().mainBuyShop.addShopItem(shopItem);

            ShopItemInfo shopItem3 = new ShopItemInfo();
            shopItem.name = "Legendary Crate";
            shopItem.cost = 3000;
            FindObjectOfType<canvas>().mainBuyShop.addShopItem(shopItem);
        }
        NumberOfLootCrates -= 1;
        FindObjectOfType<ShopManager>().lowerAmountOfCrate(rarity);
    }

    public static void addMine(int amount = 1)
    {
        if (mineAmount + 1 >= maxMines)
        {
            mineAmount = maxMines;
            FindObjectOfType<canvas>().mainBuyShop.RemoveItem("Buy Mine");
        }
        else
        {
            mineAmount += 1;
        }

    }

    public static void useMine()
    {
        if (mineAmount == maxMines)
        {
            ShopItemInfo shopItem = new ShopItemInfo();
            shopItem.name = "Buy Mine";
            shopItem.cost = 150;
            FindObjectOfType<canvas>().mainBuyShop.addShopItem(shopItem);
        }
        mineAmount -= 1;
    }
}
