using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().loseHealth(collision.gameObject.GetComponent<Player>().maxHealth);
            collision.gameObject.GetComponent<Player>().isHome = false;
        }
    }
}
