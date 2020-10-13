using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class HomePlanet : MonoBehaviour
{
    public List<(rsrce, int)> items;
    public int numberOfItemSlots = 3;
    public string name;

    // a list of the items that the planet is producing
    // (item, amount produced, frequency (seconds))
    public List<ProductionItem> productionItems;

    // Start is called before the first frame update
    void Start()
    {
        items = new List<(rsrce, int)>();
        for (int item = 0; item < numberOfItemSlots; item++)
        {
            items.Add((null, 0));
        }
    }

    // add a quantity of an item to a specified item slot
    // returns true if successful, false otherwise
    public bool addItem(rsrce item, int quantity, int itemSlot)
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

    public bool addItem(rsrce item, int quantity)
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
