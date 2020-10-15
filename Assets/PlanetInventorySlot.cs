using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetInventorySlot : MonoBehaviour
{
    public int itemSlot;
    public GameObject item; // resource
    public GameObject icon;
    public GameObject home;
    public void Start()
    {
        home = FindObjectOfType<Home>().gameObject;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            GameObject tempItem = eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().item;
            eventData.pointerDrag.transform.parent.GetComponent<PlanetInventorySlot>().RemoveItem();
            AddItemFromOldSlot(tempItem, eventData.pointerDrag);

            GameObject planet = home.GetComponent<Home>().getCurrentViewingPlanet();
            planet.GetComponent<HomePlanet>().addItem(tempItem, 1, itemSlot);
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
