using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LevelTree : MonoBehaviour
{
    public List<GameObject> purchasedItems;
    public List<GameObject> availableItems;
    public List<GameObject> lockedItems;
    public List<GameObject> planetLockedItems; // if the planet's parent(s) are purchased, but the planet isnt discovered
    public Player player;

    public void Save()
    {
        List<int> purchasedItemsIDs = new List<int>();
        foreach (GameObject item in purchasedItems)
        {
            purchasedItemsIDs.Add(item.GetComponent<TreeEntry>().ID);
        }
        FileStream fs = new FileStream("savedLevelData.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, purchasedItemsIDs);
        fs.Close();
    }

    public void Load()
    {
        List<int> purchasedItemsIDs;
        using (Stream stream = File.Open("savedLevelData.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();

            purchasedItemsIDs = (List<int>)bformatter.Deserialize(stream);
        }

        foreach (int ID in purchasedItemsIDs)
        {
            Debug.Log(ID);
            List<GameObject> unlockableItems = new List<GameObject>();
            unlockableItems.AddRange(planetLockedItems);
            unlockableItems.AddRange(availableItems);

            foreach (GameObject item in unlockableItems)
            {
                if (item.GetComponent<TreeEntry>().ID == ID)
                {
                    Debug.Log(item.GetComponent<TreeEntry>().lockedByPlanet);
                    if (item.GetComponent<TreeEntry>().lockedByPlanet)
                    {
                        Debug.Log("Here");
                        unlockPlanetItem(item);
                        purchaseItem(item);
                    }
                    else
                    {
                        purchaseItem(item);
                    }
                    break;
                }
            }
        }
    }

    private void Start()
    {
        foreach (GameObject item in lockedItems)
        {
            item.transform.Find("Lock").gameObject.SetActive(true);
        }
        foreach (GameObject item in purchasedItems)
        {
            item.transform.Find("Purchased").gameObject.SetActive(true);
        }
    }

    private void unlockItem(GameObject item)
    {
        if (lockedItems.Contains(item))
        {
            if (item.GetComponent<TreeEntry>().lockedByPlanet) {
                lockedItems.Remove(item);
                planetLockedItems.Add(item);
                return;
            }
            item.transform.Find("Lock").gameObject.SetActive(false);
            lockedItems.Remove(item);
            availableItems.Add(item);
        }
    }

    public void unlockPlanetItem(GameObject item)
    {
        if (planetLockedItems.Contains(item))
        {
            item.transform.Find("Lock").gameObject.SetActive(false);
            planetLockedItems.Remove(item);
            availableItems.Add(item);
        }
    }

    public void purchaseItem(GameObject item)
    {
        if (availableItems.Contains(item) && player.skillPoints > 0)
        {
            player.useSkillPoints(1);
            if (!item.GetComponent<TreeEntry>().repeatable)
            {
                item.transform.Find("Purchased").gameObject.SetActive(true);
                availableItems.Remove(item);
                purchasedItems.Add(item);
            }

            foreach(GameObject childItem in item.GetComponent<TreeEntry>().children)
            {
                unlockItem(childItem);
            }
            processPurchase(item.GetComponent<TreeEntry>().action);
        }
    }

    private void processPurchase(string action)
    {
        FindObjectOfType<LevelUpManager>().levelUp(action);
    }


}
