﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{

    public GameObject textPopup;
    public void movePopup(string input = "", string whichType = "", GameObject gameObjectPopup = null)
    {
        if (input == "")
        {
            textPopup.SetActive(false);
            return;
        }
        Vector3 position = Input.mousePosition;

        if (whichType == "resource")
        {
            if (position.y > 500)
            {
                position.y -= 75;
            }
            else
            {
                position.y += 75;
            }
        }

        else if(whichType == "Inventory")
        {
            position.x = gameObjectPopup.transform.position.x + gameObjectPopup.GetComponent<RectTransform>().rect.width / 2;
            position.y = gameObjectPopup.transform.position.y + gameObjectPopup.GetComponent<RectTransform>().rect.height/2 + gameObjectPopup.GetComponent<RectTransform>().rect.height / 2;
        }

        Debug.Log(input);
        position.z = 0;
        textPopup.SetActive(true);
        textPopup.transform.Find("Text").GetComponent<Text>().text = input;
        textPopup.transform.position = position;
    }
}
