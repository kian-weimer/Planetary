using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public Dictionary<string, int> resourceList = new Dictionary<string, int>();

    public void addItem(string nameOfResource)
    {
        if (resourceList.ContainsKey(nameOfResource))
        {
            resourceList[nameOfResource] = resourceList[nameOfResource] + 1;
        }
        else
        {
            resourceList.Add(nameOfResource, 1);
        }
    }

    public bool checkForItemAndRemove(string nameOfResource, int numberOfItems = 1)
    { 
        if (!resourceList.ContainsKey(nameOfResource))
        {
            return false;
        }

        if(resourceList[nameOfResource] >= numberOfItems)
        {
            resourceList[nameOfResource] = resourceList[nameOfResource] - numberOfItems;
            return true;
        }
        return false;
    }
}
