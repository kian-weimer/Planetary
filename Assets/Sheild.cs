using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheild : MonoBehaviour
{
    public GameObject shieldBar;
    public float damageAmmountFromShip;
    public float damageAmmountFromBullet;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            FindObjectOfType<Player>().sheild = FindObjectOfType<Player>().sheild - damageAmmountFromShip;
            collision.gameObject.GetComponent<EnemyController>().damage(collision.gameObject.GetComponent<EnemyController>().maxHealth);
            shieldBar.GetComponent<RectTransform>().localScale = new Vector3(FindObjectOfType<Player>().sheild / FindObjectOfType<Player>().maxSheild,
                shieldBar.GetComponent<RectTransform>().localScale.y,
                shieldBar.GetComponent<RectTransform>().localScale.z);
        }
        if(collision.gameObject.tag == "enemyBullet")
        {
            FindObjectOfType<Player>().sheild = FindObjectOfType<Player>().sheild - damageAmmountFromBullet;
            shieldBar.GetComponent<RectTransform>().localScale = new Vector3(FindObjectOfType<Player>().sheild / FindObjectOfType<Player>().maxSheild,
                shieldBar.GetComponent<RectTransform>().localScale.y,
                shieldBar.GetComponent<RectTransform>().localScale.z);
        }
    }
}
