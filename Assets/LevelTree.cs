using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTree : MonoBehaviour
{
    public List<GameObject> purchasedItems;
    public List<GameObject> availableItems;
    public List<GameObject> lockedItems;
    public List<GameObject> planetLockedItems; // if the planet's parent(s) are purchased, but the planet isnt discovered
    public Player player;

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
