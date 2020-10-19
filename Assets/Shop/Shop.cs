﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Font font;
    public List<ShopItemInfo> items; // first is name second is cost third is function 
    public List<GameObject> itemsHudObjects; // first is name second is cost third is function 

    public int itemDistance = 1;
    public GameObject text;
    public GameObject shopItem;
    private RectTransform rT;
    public GameObject Money;
    public GameObject ShopManager;
    // Start is called before the first frame update
    void Start()
    {
        
        rT = GetComponent<RectTransform>();
        // rT.sizeDelta = new Vector2(rT.sizeDelta.x, items.Count * itemDistance);
        for (int i = 0; i < items.Count; i++)
        {
           
            GameObject item = Instantiate(shopItem);
            item.name = "Item " + i;
            item.transform.parent = (transform);
            item.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -1*i * itemDistance, 0); // not sure why negative numbers there are needed...

            //item.GetComponent<RectTransform>().position = new Vector3(0, 0 + i * itemDistance , 0); // not sure why negative numbers there are needed...

            if (items[i].sellItem)
            {
                item.GetComponent<SellShopItem>().Generate(items[i]);
            }
            else
            {
                item.GetComponent<ShopItem>().Generate(items[i]);
            }
            itemsHudObjects.Add(item);
        }
    }

    public void changePrice(string name, int newPrice)
    {
        int i = 0;
        foreach(ShopItemInfo item in items)
        {
            if (item.name == name)
            {
                item.cost = newPrice;
                if (item.sellItem)
                {
                    itemsHudObjects[i].transform.Find("Value").GetComponent<Text>().text = "$" + newPrice;
                }
                else
                {
                    itemsHudObjects[i].transform.Find("Cost").GetComponent<Text>().text = "$" + newPrice;
                }
            }
            i++;
        }
    }

    public void RemoveItem(string name)
    {
        int i = 0;
        bool shift = false;
        foreach (ShopItemInfo item in items)
        {
            if (item.name == name)
            {
                items.RemoveAt(i);
                itemsHudObjects.RemoveAt(i);
                shift = true;
            }
            else if (shift)
            {
                itemsHudObjects[i].transform.position += new Vector3(0, itemDistance, 0);
            }
            i++;
        }
    }

    public void ItemPurchased(ShopItem item)
    {
        if (Money.GetComponent<Money>().removeMoney(item.cost))
        {
            ShopManager.GetComponent<ShopManager>().buyShopResultOf(item.name);
        }
    }

    public void ItemSold(SellShopItem item)
    {
        ShopManager.GetComponent<ShopManager>().sellShopResultOf(item);
    }

    public void addShopItem(ShopItemInfo item)
    {
        items.Add(item);
        GameObject itemShop = Instantiate(shopItem);
        itemShop.name = "Item " + (items.Count - 1);
        itemShop.transform.parent = transform;
        itemShop.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -1 * (items.Count - 1) * itemDistance, 0); // not sure why negative numbers there are needed...

        if (item.sellItem)
        {
            itemShop.GetComponent<SellShopItem>().Generate(item);
        }
        else
        {
            itemShop.GetComponent<ShopItem>().Generate(item);
        }
        itemsHudObjects.Add(itemShop);
    }
}
