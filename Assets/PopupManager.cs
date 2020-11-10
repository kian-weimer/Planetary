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
            if (position.y > 0)
            {
                position.y -= 100;
            }
            else
            {
                position.y += 100;
            }

            if (position.x > 0)
            {
                position.x -= 100;
            }
            else if (position.x < 0)
            {
                position.x += 100;
            }
        }
        //check if above half then make it go above
        //do right and left as well
        position.z = 0;
        textPopup.SetActive(true);
        textPopup.transform.Find("Text").GetComponent<Text>().text = input;
        textPopup.transform.position = position;
    }
}
