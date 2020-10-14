using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlanetInventorySlot : MonoBehaviour
{
    public GameObject item; // resource
    public GameObject icon;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        if (eventData.pointerDrag != null)
        {
            Debug.Log("ITEMDROPPEDONPLANETSLOT");
            GameObject tempItem = eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().item;
            eventData.pointerDrag.transform.parent.GetComponent<PlanetInventorySlot>().RemoveItem();
            AddItemFromOldSlot(tempItem, eventData.pointerDrag);

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
