using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PopupManager popupManager;
    public static GameObject mouseOverObject;
    void Start()
    {
        popupManager = FindObjectOfType<PopupManager>();
    }

    void Update()
    {
        if (mouseOverObject == null)
        {
            popupManager.movePopup();
        }
        
    }

    void OnMouseOver()
    {
        Debug.Log("Name:" + gameObject.tag + " ??:" + GameManager.popupsOn);
        if (GameManager.popupsOn)
        {
            Debug.Log(gameObject.tag);
            if (gameObject.tag == "resource")
            {
                popupManager.movePopup(gameObject.GetComponent<rsrce>().nameOfResource, "resource");
                mouseOverObject = gameObject;
            }
            if (gameObject.tag == "Planet" || gameObject.tag == "HomePlanet")
            {
                string nameOfPlanet = gameObject.GetComponent<Planet>().name.Replace("(Clone)", "");
                Debug.Log("name: " + nameOfPlanet);
                nameOfPlanet = Regex.Replace(nameOfPlanet, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
                mouseOverObject = gameObject;
                popupManager.movePopup(nameOfPlanet, "planet");
            }
        }
    }
    private void OnMouseExit()
    {
        FindObjectOfType<PopupManager>().movePopup();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (GameManager.popupsOn)
        {
            if(gameObject.tag != "Planet")
            {
                mouseOverObject = gameObject;
                string nameOfResource = gameObject.name.Replace("UI(Clone)", "");
                popupManager.movePopup(nameOfResource, "Inventory", gameObject);
            }
        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOverObject = null;
    }
}
