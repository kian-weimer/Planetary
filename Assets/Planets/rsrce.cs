using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class rsrce : MonoBehaviour
{
    public bool UIElement = false;
    public GameObject icon;
    public string nameOfResource;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!UIElement)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * .99f;
            GetComponent<Rigidbody2D>().angularVelocity = GetComponent<Rigidbody2D>().angularVelocity * .99f;
        }
    }

    public void Pickedup()
    {

        //FindObjectOfType<canvas>().transform.Find("ItemImage").GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        //FindObjectOfType<canvas>().transform.Find("ItemImage").GetComponent<CanvasGroup>().alpha = 1;
        transform.position = new Vector3(0, 0, 1);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
