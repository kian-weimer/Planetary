using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupManager : MonoBehaviour
{

    public GameObject textPopup;
    public void movePopup(string input = "", string whichType = "")
    {
        if (input == "")
        {
            textPopup.SetActive(false);
            return;
        }
        Vector3 position = Input.mousePosition;

        if (whichType == "resource")
        {
            Debug.Log(position.y);
            if (position.y > 500)
            {
                position.y -= 75;
            }
            else
            {
                position.y += 75;
            }
        }
        Debug.Log(input);
        position.z = 0;
        textPopup.SetActive(true);
        textPopup.transform.Find("Text").GetComponent<Text>().text = input;
        textPopup.transform.position = position;
    }
}
