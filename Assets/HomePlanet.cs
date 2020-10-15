using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.UI;

public class HomePlanet : MonoBehaviour
{
    public List<(GameObject, int)> items;
    public int numberOfItemSlots = 3;
    public string name;

    public GameObject planetHUD;

    // a list of the items that the planet is producing
    // (item, amount produced, frequency (seconds))
    public List<ProductionItem> productionItems;

    // Start is called before the first frame update
    void Start()
    {
        planetHUD = transform.parent.GetComponent<Home>().planetHUD;
        items = new List<(GameObject, int)>();
        for (int item = 0; item < numberOfItemSlots; item++)
        {
            items.Add((null, 0));
        }

    }

    public void UpdateUI()
    {
        for (int item = 0; item < numberOfItemSlots; item++)
        {
            planetHUD.transform.Find("Item" + (item + 1)).Find("Quantity").gameObject.GetComponent<Text>().text = "X" + (items[item].Item2 + "").PadLeft(2, '0');
            foreach (Transform child in planetHUD.transform.Find("Item" + (item + 1)).transform)
            {
                if (child.name != "Quantity")
                {
                    Debug.Log(child.name);
                    GameObject.Destroy(child.gameObject);
                }
            }
            planetHUD.transform.Find("Item" + (item + 1)).GetComponent<PlanetInventorySlot>().RemoveItem();
            if (items[item].Item1 != null)
            {
                planetHUD.transform.Find("Item" + (item + 1)).GetComponent<PlanetInventorySlot>().AddItem(items[item].Item1);
            }
           
        }   
    }

    // add a quantity of an item to a specified item slot
    // returns true if successful, false otherwise
    public bool addItem(GameObject item, int quantity, int itemSlot)
    {
        if (items[itemSlot].Item1 == null)
        {
            items[itemSlot] = (item, quantity);
            return true;
        }
        else if (items[itemSlot].Item1 == item)
        {
            items[itemSlot] = (item, items[itemSlot].Item2 + quantity);
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
            if (items[itemSlot].Item1 == item)
            {
                items[itemSlot] = (item, items[itemSlot].Item2 + quantity);
                return true;
            }
        }

        // loop through, find first null slot, add item and quantity if successful
        for (int itemSlot = 0; itemSlot < numberOfItemSlots; itemSlot++)
        {
            if (items[itemSlot].Item1 == null)
            {
                items[itemSlot] = (item, quantity);
                return true;
            }
        }

        // no item slots available
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
