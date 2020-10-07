using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    public Camera cameraWindow;
    bool draging = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && gameObject.activeSelf && !draging && 
            !RectTransformUtility.RectangleContainsScreenPoint(
                gameObject.GetComponent<RectTransform>(),
                Input.mousePosition))
        {
            transform.parent.gameObject.SetActive(false);
        }
        else if (Input.GetMouseButton(0) && gameObject.activeSelf)
        {
            draging = true;
        }
        else if (draging && !Input.GetMouseButton(0) && gameObject.activeSelf)
        {
            draging = false;
        }
    }
}
