using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;


public class rsrce : MonoBehaviour
{
    public bool UIElement = false;
    public GameObject icon;
    public string nameOfResource;
    public bool isInAnInventory = false;
    public float timeBeforeDeletion;

    private float timeLeft;

    void Start()
    {
        if (GameManager.lightsOn)
        {
            GetComponent<Light2D>().enabled = true;
        }
        timeLeft = timeBeforeDeletion;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!UIElement)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity * .999f;
            GetComponent<Rigidbody2D>().angularVelocity = GetComponent<Rigidbody2D>().angularVelocity * .9995f;
        }

        if(isInAnInventory == false)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Pickedup()
    {
        isInAnInventory = true;
        timeLeft = timeBeforeDeletion;
        transform.position = new Vector3(0, 0, 1);
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
