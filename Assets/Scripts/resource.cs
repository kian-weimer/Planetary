using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pickedup()
    {
        Debug.Log("ahhh");
        FindObjectOfType<canvas>().transform.Find("ItemImage").GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        //Destroy(gameObject);
    }
}
