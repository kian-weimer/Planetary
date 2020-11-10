using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour, IDropHandler
{
    public ResourceInventory RI;

    public GameObject item; // resource
    public GameObject icon;

    public string select; // planet, enemy, resource

    public string desiredResource;
    public int desiredQuantity;
    public int quantity = 0;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Dropped");
        if (select == "resource" &&
            eventData.pointerDrag != null && 
            desiredResource.ToUpper().Contains(eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().item.GetComponent<rsrce>().nameOfResource.ToUpper())
            && quantity < desiredQuantity)
        {

            if (eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>() != null)
            {
                GameObject tempItem = eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().item;
                eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().RemoveItem();
                AddItemFromOldSlot(tempItem, eventData.pointerDrag);
                FindObjectOfType<Inventory>().isFull = false; // inventory must no longer be full
            }
            else
            {
                GameObject tempItem = eventData.pointerDrag.transform.parent.GetComponent<PlanetInventorySlot>().item;
                eventData.pointerDrag.transform.parent.GetComponent<PlanetInventorySlot>().home.GetComponent<Home>().
                    homePlanets[eventData.pointerDrag.transform.parent.GetComponent<PlanetInventorySlot>().home.GetComponent<Home>()
                    .currentViewingPlanet].GetComponent<HomePlanet>().removeItem(eventData.pointerDrag.transform.parent.
                    GetComponent<PlanetInventorySlot>().itemSlot);

                eventData.pointerDrag.transform.parent.GetComponent<PlanetInventorySlot>().RemoveItem();
                AddItemFromOldSlot(tempItem, eventData.pointerDrag);
                Debug.Log(tempItem);
            }

            // reduce inventory here
            RI.resourceList[item.GetComponent<rsrce>().nameOfResource] -= 1;

            icon.GetComponent<DragDrop>().destroy();
            Destroy(item);
            RemoveItem();

            // increase quest count here
            increaseCount();
        }
    }

    public void increaseCount()
    {
        quantity++;
        transform.parent.Find("Quantity").GetComponent<Text>().text = quantity + "/" + desiredQuantity;
        transform.parent.parent.GetComponent<QuestEntry>().updateTracking();
        if (quantity == desiredQuantity)
        {
            transform.parent.parent.GetComponent<QuestEntry>().taskComplete();
        }
    }

    public void AddItem(GameObject item)
    {
        this.item = item;
        icon = Instantiate(item.GetComponent<rsrce>().icon);
        icon.transform.parent = transform;
        icon.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0); // GetComponent<RectTransform>().anchoredPosition;
    }

    public void AddItemFromOldSlot(GameObject item, GameObject icon)
    {
        this.item = item;
        this.icon = icon;
        icon.GetComponent<DragDrop>().moved = true;
        icon.transform.parent = transform;
        icon.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0); //GetComponent<RectTransform>().anchoredPosition;
    }


    public void RemoveItem()
    {
        icon = null;
        item = null;
    }
}
