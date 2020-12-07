using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Canvas canvas;
    RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public bool moved;

    public GameObject tempUI;
    private void Awake()
    {
        canvas = FindObjectOfType<canvas>().GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        transform.parent.SetAsLastSibling(); // keps icon on top
        transform.parent.parent.SetAsLastSibling(); // keps icon more on top
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.parent.localScale;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (moved)
        {
            moved = false;
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        BlockShooting.forceBlocking = true;
        if (transform.parent.parent.Find("DeleteSlot") != null)
        {
            transform.parent.parent.Find("DeleteSlot").gameObject.SetActive(true);
        }
        if (transform.parent.parent.Find("OxygenSlot") != null && transform.parent.parent.Find("OxygenSlot").GetComponent<OxygenSlot>().eventOcurring)
        {
            transform.parent.parent.Find("OxygenSlot").gameObject.SetActive(true);
        }
        if (transform.parent.GetComponent<PlanetInventorySlot>() != null)
        {
            GameObject planet = FindObjectOfType<Home>().GetComponent<Home>().getCurrentViewingPlanet();

            if (planet.GetComponent<HomePlanet>().items[transform.parent.GetComponent<PlanetInventorySlot>().itemSlot].quantity > 1)
            {
                tempUI = Instantiate(transform.parent.GetComponent<PlanetInventorySlot>().icon);
                tempUI.transform.parent = transform.parent;
                tempUI.GetComponent<RectTransform>().localScale = gameObject.GetComponent<RectTransform>().localScale;
                tempUI.GetComponent<RectTransform>().position = transform.position;
            }
        }

    }

    public void OnDrop(PointerEventData eventData)
    {
        // stackable if on the planet inventory
        // 
        if (eventData.pointerDrag != null && eventData.pointerDrag.transform.parent.GetComponent<PlanetInventorySlot>() == null && transform.parent.GetComponent<PlanetInventorySlot>() != null && transform.parent.GetComponent<InventorySlot>() == null)
        {
            GameObject tempItem;

            // dont allow switching in planet inventory slots
            //Debug.Log(eventData.pointerDrag.name.Replace("(Clone)", "") + transform.name.Replace("(Clone)", ""));

            if (eventData.pointerDrag.name.Replace("(Clone)", "") == transform.name.Replace("(Clone)", ""))
            {
                GameObject planet = FindObjectOfType<Home>().GetComponent<Home>().getCurrentViewingPlanet();
                tempItem = transform.parent.GetComponent<PlanetInventorySlot>().item;
                planet.GetComponent<HomePlanet>().addItem(tempItem, 1, transform.parent.GetComponent<PlanetInventorySlot>().itemSlot);
                planet.GetComponent<HomePlanet>().UpdateUI();
                eventData.pointerDrag.transform.parent.GetComponent<InventorySlot>().RemoveItem();
                Destroy(eventData.pointerDrag);
            }
        }
        return;
        // stackable if on the planet inventory
        // 
        if (eventData.pointerDrag != null && transform.parent.GetComponent<PlanetInventorySlot>() != null && transform.parent.GetComponent<InventorySlot>() == null)
        {
            GameObject tempItem;
            GameObject planet = FindObjectOfType<Home>().GetComponent<Home>().getCurrentViewingPlanet();

            // dont allow switching in planet inventory slots
            if (eventData.pointerDrag.name.Replace("(Clone)", "") == transform.name.Replace("(Clone)", ""))
            {
                tempItem = transform.parent.GetComponent<PlanetInventorySlot>().item;
                planet.GetComponent<HomePlanet>().addItem(tempItem, 1, transform.parent.GetComponent<PlanetInventorySlot>().itemSlot);
                Destroy(eventData.pointerDrag);
            }

            planet.GetComponent<HomePlanet>().UpdateUI();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        BlockShooting.forceBlocking = false;
        if (transform.parent.parent.Find("DeleteSlot") != null)
        {
            transform.parent.parent.Find("DeleteSlot").GetComponent<DeleteSlot>().delete();
        }
        if (transform.parent.parent.Find("OxygenSlot") != null )
        {
            transform.parent.parent.Find("OxygenSlot").GetComponent<OxygenSlot>().delete();
        }
        return;
        //FindObjectOfType<Home>().GetComponent<Home>().getCurrentViewingPlanet().GetComponent<HomePlanet>().UpdateUI();
        if (tempUI != null)
        {
            Destroy(tempUI);
        }
    }

    public void destroy()
    {
        Destroy(gameObject);
    }
}


