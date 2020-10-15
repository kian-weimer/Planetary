﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public GameObject item; // resource
    public GameObject icon;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("HERE");
        if (eventData.pointerDrag != null)
        {
            GameObject tempItem = eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().item;
            eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().RemoveItem();
            AddItemFromOldSlot(tempItem, eventData.pointerDrag);
        }
    }

    public void AddItem(GameObject item)
    {
        this.item = item;
        icon = Instantiate(item.GetComponent<rsrce>().icon);
        icon.transform.parent = transform;
        icon.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0); // GetComponent<RectTransform>().anchoredPosition;
        Debug.Log("Item Added");
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
