﻿﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class HomePlanet : MonoBehaviour
{
    public List<PlanetResource> items;
    public int numberOfItemSlots = 3;
    public string name;

    public GameObject planetHUD;

    // a list of the items that the planet is producing
    // (item, amount produced, frequency (seconds))
    public List<ProductionItem> productionItems;
    public bool[] hasProducedItem;
    public ResourceInventory resourceInventory;


    // Start is called before the first frame update
    void Start()
    {
        productionItems = new List<ProductionItem>();
        resourceInventory = FindObjectOfType<ResourceInventory>();
        hasProducedItem = new bool[5];
        planetHUD = transform.parent.GetComponent<Home>().planetHUD;
        items = new List<PlanetResource>();
        for (int item = 0; item < numberOfItemSlots; item++)
        {
            items.Add(new PlanetResource());
        }

    }

    public void UpdateUI()
    {
        for (int item = 0; item < numberOfItemSlots; item++)
        {
            planetHUD.transform.Find("Item" + (item + 1)).Find("Quantity").gameObject.GetComponent<Text>().text = "X" + (items[item].quantity + "").PadLeft(2, '0');
            foreach (Transform child in planetHUD.transform.Find("Item" + (item + 1)).transform)
            {
                if (child.name != "Quantity")
                {
                    //Debug.Log(child.name);
                    GameObject.Destroy(child.gameObject);
                }
            }
            planetHUD.transform.Find("Item" + (item + 1)).GetComponent<PlanetInventorySlot>().RemoveItem();
            if (items[item].resource != null)
            {
                planetHUD.transform.Find("Item" + (item + 1)).GetComponent<PlanetInventorySlot>().AddItem(items[item].resource);
            }

        }
    }

    public void UpdateUI(int itemSlot)
    {
        planetHUD.transform.Find("Item" + (itemSlot + 1)).Find("Quantity").gameObject.GetComponent<Text>().text = "X" + (items[itemSlot].quantity + "").PadLeft(2, '0');
    }

    // add a quantity of an item to a specified item slot
    // returns true if successful, false otherwise
    public bool addItem(GameObject item, int quantity, int itemSlot)
    {
        if (items[itemSlot].resource == null)
        {
            items[itemSlot].Set(item, quantity);
            return true;
        }
        else if (items[itemSlot].resource.name.Replace("(Clone)", "") == item.name.Replace("(Clone)", "") == item)
        {
            items[itemSlot].Set(item, items[itemSlot].quantity + quantity);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool addItem(GameObject item, int quantity)
    {
        // loop through, check for item, add quantity if found
        for (int itemSlot = 0; itemSlot < numberOfItemSlots; itemSlot++)
        {
            if (items[itemSlot].resource==null) { continue; }
            if (items[itemSlot].resource.name.Replace("(Clone)", "") == item.name.Replace("(Clone)", ""))
            {
                items[itemSlot].Set(item, items[itemSlot].quantity + quantity);
                return true;
            }
        }

        // loop through, find first null slot, add item and quantity if successful
        for (int itemSlot = 0; itemSlot < numberOfItemSlots; itemSlot++)
        {
            if (items[itemSlot].resource == null)
            {
                items[itemSlot].Set(item, quantity);
                return true;
            }
        }

        // no item slots available
        return false;
    }

    
    public bool addItem(rsrce item, int quantity)
    {
        // loop through, check for item, add quantity if found
        for (int itemSlot = 0; itemSlot < numberOfItemSlots; itemSlot++)
        {
            if (items[itemSlot].resource == null) { continue; }
            if (items[itemSlot].resource.name.Replace("(Clone)", "") == item.name.Replace("(Clone)", ""))
            {
                items[itemSlot].Set(items[itemSlot].resource, items[itemSlot].quantity + quantity);
                return true;
            }
        }

        // loop through, find first null slot, add item and quantity if successful
        for (int itemSlot = 0; itemSlot < numberOfItemSlots; itemSlot++)
        {
            if (items[itemSlot].resource == null)
            {
                items[itemSlot].Set(Instantiate(item).gameObject, quantity);
                return true;
            }
        }

        // no item slots available
        return false;
    }

    public void removeItem(int itemSlot, int quantity)
    {
        if (items[itemSlot].quantity > quantity)
        {
            items[itemSlot].quantity -= quantity;
        }
        else if (items[itemSlot].quantity == quantity)
        {
            items[itemSlot].Set(null, 0);
        }

        UpdateUI(itemSlot);
    }

    public void removeItem(int itemSlot)
    {
        if (items[itemSlot].quantity > 1)
        {
            items[itemSlot].quantity -= 1;
        }
        else
        {
            items[itemSlot].Set(null, 0);
        }
        
        UpdateUI(itemSlot);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int i = 0;
        foreach(ProductionItem prodItem in productionItems) 
        { 
            if ((int)Time.time % prodItem.frequency == 0 && !hasProducedItem[i])
            {
                hasProducedItem[i] = true;
                if (!addItem(prodItem.resource.gameObject, prodItem.amountProduced))
                {
                    Debug.LogWarning("PLANET FULL, CANNOT PRODUCE RESOURCES!!");
                }
                else
                {
                    resourceInventory.addItem(prodItem.resource.GetComponent<rsrce>().nameOfResource, prodItem.amountProduced);
                }
                transform.parent.GetComponent<Home>().UpdatePlanetHud();
            }
            else if ((int)Time.time % prodItem.frequency != 0)
            {
                hasProducedItem[i] = false;
            }
            i++;
        }
    }
}