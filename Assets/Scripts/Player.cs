
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerInfo info;

    public int maxHealth;

    [HideInInspector]
    public int health; // player's health

    public Weapon weapon;

    public Vector3 position;
    public Rigidbody2D rb;
    public GameObject resourceInShip;

    // Start is called before the first frame update
    void Start()
    {
        resourceInShip = null;
        health = maxHealth;
        info = new PlayerInfo(0, 0, maxHealth, health);
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
        //Debug.Log(direction);

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
            collision.gameObject.GetComponent<Resource>().Pickedup();
        }
        if (collision.gameObject.tag == "Popup")
        {

            collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject.SetActive(true);



        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject.activeSelf)
        {
            collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject.SetActive(false);
        }
    }
}
