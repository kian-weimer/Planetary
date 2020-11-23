using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tradeCollider : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            FindObjectOfType<canvas>().enableFriendlyTraderShopButton();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            FindObjectOfType<canvas>().disableFriendlyTraderShopButton();
        }
    }
}