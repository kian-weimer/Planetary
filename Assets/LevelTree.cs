using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTree : MonoBehaviour
{
    public List<GameObject> purchasedItems;
    public List<GameObject> availableItems;
    public List<GameObject> lockedItems;

    private void Start()
    {
        foreach(GameObject item in lockedItems)
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
            item.transform.Find("Lock").gameObject.SetActive(false);
            lockedItems.Remove(item);
            availableItems.Add(item);
        }
    }

    public void purchaseItem(GameObject item)
    {
        if (availableItems.Contains(item))
        {
            item.transform.Find("Purchased").gameObject.SetActive(true);
            availableItems.Remove(item);
            purchasedItems.Add(item);
            foreach(GameObject childItem in item.GetComponent<TreeEntry>().children)
            {
                unlockItem(childItem);
            }
            processPurchase(item.GetComponent<TreeEntry>().action);
        }
    }

    private void processPurchase(string action)
    {
        Debug.Log(action);
    }


}
