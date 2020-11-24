using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public GameObject item; // resource
    public GameObject icon;
    public bool oxygenSlot = false;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null )
        {
            if (!eventData.pointerDrag.name.Contains("Oxygen") && oxygenSlot)
            {
                return;
            }
            if (eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>() != null)
            {
                GameObject tempItem = eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().item;
                eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().RemoveItem();
                AddItemFromOldSlot(tempItem, eventData.pointerDrag);
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
            }
            
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
        if (icon.GetComponent<DragDrop>() != null)
        {
            icon.GetComponent<DragDrop>().moved = true;
        }
        else
        {
            icon.GetComponent<DragDropSpawnable>().moved = true;
        }
        icon.transform.parent = transform;
        icon.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0); //GetComponent<RectTransform>().anchoredPosition;
    }


    public void RemoveItem()
    {
        icon = null;
        item = null;
        transform.parent.gameObject.GetComponent<Inventory>().isFull = false;
    }

}

