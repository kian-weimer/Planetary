
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public PlayerInfo info;

    public Camera main;
    public Camera planetView;

    public int maxHealth;
    public float maxGas;
    public GameObject gasBar;
    public GameObject gasBarEnd;

    public GameObject healthBar;
    public GameObject healthBarEnd;

    //[HideInInspector]
    public float health; // player's health
    //[HideInInspector]
    public float gas; // player's health

    public float fuelConsumption;

    public GameObject weapon;
    public List<GameObject> weapons;

    public Vector3 position;
    public Rigidbody2D rb;
    public GameObject resourceInShip;

    public AudioManager audioManager;

    public bool isHome = true;
    public bool canShoot = true;
    public bool disabled = false;

    public float regenRate;
    public Inventory inventory;

    public GameObject canvas;
    public GameObject shop;
    public GameObject menuButton;

    public int experiencePoints = 0;
    public int level = 1;
    public int pointsToNextLevel = 100;
    public GameObject ExpBar;

    public GameObject tempWeapon2; // dont create the weapons in player, this is just for testing.

    // Start is called before the first frame update
    void Start()
    {
        resourceInShip = null;
        health = maxHealth;
        gas = maxGas;
        info = new PlayerInfo(0, 0, maxHealth, health, maxGas, gas);
        weapon = Instantiate(weapon).gameObject;
        weapons.Add(weapon);
        weapon.transform.parent = transform;
        weapon.GetComponent<Weapon>().lastShotTime = 0;
        weapon.GetComponent<Weapon>().bullet.GetComponent<Bullet>().bulletDamage = 5;
        AddWeapon(Instantiate(tempWeapon2)); // just used for testing. Add weapon should not be called by the player
    }

    void SwitchWeapon()
    {
        if (weapons.IndexOf(weapon) < weapons.Count - 1)
        {
            weapon = weapons[weapons.IndexOf(weapon) + 1];
        }
        else
        {
            weapon = weapons[0];
        }
    }

    void AddWeapon(GameObject weapon)
    {
        weapon.transform.parent = transform;
        weapons.Add(weapon);
        this.weapon = weapon;
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
        if (Input.GetKey(KeyCode.Mouse0) && main.gameObject.activeSelf && canShoot)
        {
            float rotation = (rb.rotation + 90);
            if (weapon.GetComponent<Weapon>().Shoot(transform.Find("Barrel").position, new Vector2(Mathf.Cos(((rotation) * Mathf.PI) / 180), Mathf.Sin(((rotation) * Mathf.PI) / 180)), gameObject.GetComponent<Rigidbody2D>().velocity)) {
                audioManager.Play("LazerShoot");
            }
        }




    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Mouse1)))
        {
            Debug.Log("WeaponSwitched");
            SwitchWeapon();
        }
        if ((Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Escape))  && nearHomePlanet && !disabled)
        {
            if (nearHomePlanet && main.gameObject.activeSelf)
            {
                main.gameObject.SetActive(false);
                planetView.gameObject.SetActive(true);
                FindObjectOfType<canvas>().transform.Find("PlanetHUD").gameObject.SetActive(true);
                FindObjectOfType<Home>().GetComponent<Home>().ChangePlanetView(closestHomePlanet.rarity);
                //planetView.transform.position = new Vector3(closestHomePlanet.transform.position.x, closestHomePlanet.transform.position.y, -10);
                popUpText.SetActive(false);
                ToggleShooting();
                gameObject.GetComponent<PlayerController>().canMove = false;
                menuButton.gameObject.SetActive(false);
            }

            else if (nearHomePlanet && planetView.gameObject.activeSelf)
            {
                planetView.gameObject.SetActive(false);
                FindObjectOfType<canvas>().transform.Find("PlanetHUD").gameObject.SetActive(false);
                main.gameObject.SetActive(true);
                popUpText.SetActive(true);
                ToggleShooting();
                gameObject.GetComponent<PlayerController>().canMove = true;
                menuButton.gameObject.SetActive(true);
            }
        }

        if (isHome)
        {
            regen();
        }
    }

    public bool nearHomePlanet = false;
    public Planet closestHomePlanet;
    public GameObject popUpText;

    public void HomePlanetDestroyed(Planet planet)
    {
        // fixes UI
        if (planet == closestHomePlanet)
        {
            closestHomePlanet = null;
            nearHomePlanet = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "resource" && !inventory.isFull)
        {
            resourceInShip = collision.gameObject;
            inventory.StoreItem(collision.gameObject);
            collision.gameObject.GetComponent<rsrce>().Pickedup();
        }

        if (collision.gameObject.tag == "Popup")
        {
            if (collision.gameObject.transform.parent.GetComponent<Planet>().inHomeSystem == true)
            {
                collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject.SetActive(true);
                nearHomePlanet = true;
                closestHomePlanet = collision.transform.parent.GetComponent<Planet>();
                popUpText = collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject;
            }
        }

        if (collision.gameObject.tag == "HomeCircle")
        {
            isHome = true;
            canvas.transform.Find("ShopButton").gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Popup")
        {
            if (collision.gameObject.transform.parent.GetComponent<Planet>().inHomeSystem == true)
            {
                if (collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject.activeSelf)
                {
                    collision.gameObject.transform.Find("Canvas").transform.Find("PopupText").gameObject.SetActive(false);
                    nearHomePlanet = false;
                    closestHomePlanet = null;
                    popUpText = null;
                }
            }
        }
        if (collision.gameObject.tag == "HomeCircle")
        {
            isHome = false;
            if (canvas.transform.Find("ShopButton").gameObject.activeSelf == true)
            {
                canvas.transform.Find("ShopButton").gameObject.SetActive(false);
                shop.gameObject.SetActive(false);
                canShoot = true;
            }
          
        }
    }

    public bool ConsumeGas(bool isBackwards)
    {
        gas -= isBackwards ? fuelConsumption / 4 : fuelConsumption;

        if (gas > 0.5)
        {
            gasBar.transform.localScale = new Vector3(1 * (gas - maxGas * .1f) / (maxGas - maxGas * .1f), gasBar.transform.localScale.y, gasBar.transform.localScale.z);

            if (gas <= maxGas * .1f)
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

            if(GetComponent<PlayerController>().hasRespawned == false)
            {
                GetComponent<Respawn>().playerRespawn();
                GetComponent<PlayerController>().hasRespawned = true;
            }
            return false;
        }
    }
    public void loseHealth(float healthLoss)
    {
        health -= healthLoss;
        if (health > 0.5)
        {
            healthBar.transform.localScale = new Vector3(1 * (health - maxHealth * .1f) / (maxHealth - maxHealth * .1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);

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

        if (health <= 0)
        {
            GetComponent<Respawn>().playerRespawn();
            GetComponent<PlayerController>().canMove = false;
        }
    }

    public void regen()
    {

        if (gas < maxGas)
        {
            gas += regenRate*maxGas;

            gasBar.transform.localScale = new Vector3(1 * (gas - maxGas * .1f) / (maxGas - maxGas * .1f), gasBar.transform.localScale.y, gasBar.transform.localScale.z);

            if (gas <= maxGas * .1f)
            {
                gasBarEnd.transform.localScale = new Vector3(-1 * (maxGas * .1f - gas) / (maxGas * .1f), gasBarEnd.transform.localScale.y, gasBarEnd.transform.localScale.z);
            }
        }
        else
        {
            gas = maxGas;
        }

        if (health < maxHealth)
        {
            health += regenRate * maxHealth;

            healthBar.transform.localScale = new Vector3(1 * (health - maxHealth * .1f) / (maxHealth - maxHealth * .1f), healthBar.transform.localScale.y, healthBar.transform.localScale.z);

            if (health <= maxHealth * .1f)
            {
                healthBarEnd.transform.localScale = new Vector3(-1 * (maxHealth * .1f - health) / (maxHealth * .1f), healthBarEnd.transform.localScale.y, healthBarEnd.transform.localScale.z);
            }
        }
        else
        {
            health = maxHealth;
        }
    }

    public void ToggleShooting()
    {
        canShoot = !canShoot;
    }

    public void addExpPoints(int points)
    {
        experiencePoints += points;
        ExpBar.transform.Find("BarBG").Find("ExpBar").localScale = new Vector3((float)experiencePoints / pointsToNextLevel, 1, 1);
        ExpBar.transform.Find("NumeratedExp").GetComponent<Text>().text = experiencePoints + "/" + pointsToNextLevel;
        if (experiencePoints >= pointsToNextLevel)
        {
            levelUp();
        }
    }

    public void levelUp()
    {
        level++;
    }
}