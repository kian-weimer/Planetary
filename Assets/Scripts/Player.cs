
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerInfo info;

    public int maxHealth;
    public float maxGas;
    public GameObject gasBar;
    public GameObject gasBarEnd;

    public GameObject healthBar;
    public GameObject healthBarEnd;

    [HideInInspector]
    public float health; // player's health
    [HideInInspector]
    public float gas; // player's health

    public float fuelConsumption;

    public Weapon weapon;

    public Vector3 position;
    public Rigidbody2D rb;
    public GameObject resourceInShip;

    public bool isHome = true;

    // Start is called before the first frame update
    void Start()
    {
        resourceInShip = null;
        health = maxHealth;
        gas = maxGas;
        info = new PlayerInfo(0, 0, maxHealth, health, maxGas, gas);
        weapon.lastShotTime = 0;
    }

    public void LoadFromInfo(PlayerInfo info)
    {
        this.info = info;
        this.health = info.health;
        this.position = new Vector3(info.position[0], info.position[1], 0);
        gameObject.transform.position = position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            float rotation = (rb.rotation + 90);
            weapon.Shoot(transform.Find("Barrel").position, new Vector2(Mathf.Cos(((rotation) * Mathf.PI) / 180), Mathf.Sin(((rotation) * Mathf.PI) / 180)));//new Vector2(Mathf.Cos(((rotation) * Mathf.PI) / 180), Mathf.Sin(((rotation) * Mathf.PI) / 180)));
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "resource" && resourceInShip == null)
        {
            resourceInShip = collision.gameObject;
            collision.gameObject.GetComponent<rsrce>().Pickedup();
        }
        if (collision.gameObject.tag == "Popup")
        {
            if (collision.gameObject.transform.parent.GetComponent<Planet>().inHomeSystem == true)
            {
                collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Popup")
        {
            if(collision.gameObject.transform.parent.GetComponent<Planet>().inHomeSystem == true)
            {
                if (collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject.activeSelf)
                {
                    collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject.SetActive(false);
                }
            }
        }
    }

    public bool ConsumeGas()
    {
        gas -= fuelConsumption;
        //gasBar.transform.localScale =  new Vector3(gasBar.transform.localScale.x * gas/maxGas, gasBar.transform.localScale.y, gasBar.transform.localScale.z);
        if (gas > 0.5)
        {
            gasBar.transform.localScale = new Vector3(1 * (gas - maxGas * .1f) / maxGas, gasBar.transform.localScale.y, gasBar.transform.localScale.z);
          
            if (gas <= maxGas*.1f)
            {
                gasBarEnd.transform.localScale = new Vector3(-1 * (maxGas * .1f - gas) / (maxGas * .1f), gasBarEnd.transform.localScale.y, gasBarEnd.transform.localScale.z);
            }
            return true;
        }
        else
        {
            gas = 0;
            gasBar.transform.localScale = new Vector3(0, gasBar.transform.localScale.y, gasBar.transform.localScale.z);
            gasBarEnd.transform.localScale = new Vector3(-1, gasBarEnd.transform.localScale.y, gasBarEnd.transform.localScale.z);
            return false;
        }
    }
    public void loseHealth(float healthLoss)
    {
        health -= healthLoss;
        if (health > 0.5)
        {
            healthBar.transform.localScale = new Vector3(1 * (health - maxHealth * .1f) / maxHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);

            if (health <= maxHealth * .1f)
            {
                healthBarEnd.transform.localScale = new Vector3(-1 * (maxHealth * .1f - health) / (maxHealth * .1f), healthBarEnd.transform.localScale.y, healthBarEnd.transform.localScale.z);
            }
        }
        else
        {
            health = 0;
            healthBar.transform.localScale = new Vector3(0, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
            healthBarEnd.transform.localScale = new Vector3(-1, healthBarEnd.transform.localScale.y, healthBarEnd.transform.localScale.z);
        }
    }
}
