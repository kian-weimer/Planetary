using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public Dictionary<string, int> resourceList = new Dictionary<string, int>();

    public List<string> resourcesSeen = new List<string>();

    public GameObject sellMenu;

    public void Save()
    {
        List<string> keys = resourceList.Keys.ToList<string>();
        List<int> values = resourceList.Values.ToList<int>();
        FileStream fs = new FileStream("savedResourceInventoryKeys.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, keys);
        fs.Close();

        FileStream fs2 = new FileStream("savedResourceInventoryValues.dat", FileMode.Create);
        BinaryFormatter bf2 = new BinaryFormatter();
        bf2.Serialize(fs2, values);
        fs2.Close();

        FileStream fs3 = new FileStream("resourcesSeen.dat", FileMode.Create);
        BinaryFormatter bf3 = new BinaryFormatter();
        bf3.Serialize(fs3, resourcesSeen);
        fs3.Close();
    }

    public void Load()
    {
        using (Stream stream = File.Open("resourcesSeen.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();

            resourcesSeen = (List<string>)bformatter.Deserialize(stream);
            Debug.Log(resourcesSeen[0] + resourcesSeen[1]);
        }

        List<string> keys;
        using (Stream stream = File.Open("savedResourceInventoryKeys.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();

            keys = (List<string>)bformatter.Deserialize(stream);
        }

        List<int> values;
        using (Stream stream = File.Open("savedResourceInventoryValues.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();

            values = (List<int>)bformatter.Deserialize(stream);
        }

        for(int i = 0; i < keys.Count; i++)
        {
            resourceList.Add(keys[i], values[i]);
        }
    }

    public void addItem(string nameOfResource, int quantity = 1)
    {
        if (resourceList.ContainsKey(nameOfResource))
        {
            resourceList[nameOfResource] = resourceList[nameOfResource] + quantity;
        }
        else
        {
            if (!resourcesSeen.Contains(nameOfResource))
            {
                resourcesSeen.Add(nameOfResource);
                ShopItemInfo sellItem = new ShopItemInfo();
                sellItem.name = nameOfResource;
                sellItem.sellItem = true;
                sellItem.cost = FindObjectOfType<ShopManager>().resourceCost[nameOfResource];

                sellMenu.GetComponent<Shop>().addShopItem(sellItem);
            }
            resourceList.Add(nameOfResource, quantity);
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
