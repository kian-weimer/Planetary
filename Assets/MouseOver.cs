using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class MouseOver : MonoBehaviour
{
    private PopupManager popupManager;
    public static GameObject mouseOverObject;
    void Start()
    {
        popupManager = FindObjectOfType<PopupManager>();
    }

    void Update()
    {
        if(mouseOverObject == null)
        {
            FindObjectOfType<PopupManager>().movePopup();
        }
    }

    void OnMouseOver()
    {

        if (gameObject.tag == "resource")
        {
            popupManager.movePopup(gameObject.GetComponent<rsrce>().nameOfResource, "resource");
            mouseOverObject = gameObject;
        }
        if (gameObject.tag == "Planet")
        {
            string nameOfPlanet = gameObject.GetComponent<Planet>().name.Replace("(Clone)", "");
            nameOfPlanet = Regex.Replace(nameOfPlanet, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
            mouseOverObject = gameObject;
            Debug.Log(nameOfPlanet);
        }
        
    }
    private void OnMouseExit()
    {
        FindObjectOfType<PopupManager>().movePopup();
    }
}
