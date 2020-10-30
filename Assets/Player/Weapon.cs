﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public float firerate; // bullets per second
    public float bulletSpeed;
    public float lastShotTime;
    public bool isInGUI = true;
    public int weaponSelect = 1;
    private void Start()
    {
        lastShotTime = Time.time;
    }

    public bool Shoot(Vector2 position, Vector2 direction, Vector2 playerSpeed)
    {
        switch (weaponSelect)
        {
            case 1: // basic shooting
                if (Time.time > lastShotTime + 1 / firerate && !isInGUI)
                {
                    lastShotTime = Time.time;
                    GameObject tempBullet = Instantiate(bullet).gameObject;
                    tempBullet.transform.position = position;
                    tempBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed + playerSpeed;
                    return true;
                }
                break;
            case 2:
                if (Time.time > lastShotTime + 1 / firerate && !isInGUI)
                {
                    lastShotTime = Time.time;
                    GameObject tempBullet = Instantiate(bullet).gameObject;
                    tempBullet.transform.position = transform.parent.Find("Barrel1").position ;
                    tempBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed + playerSpeed;
                    GameObject tempBullet2 = Instantiate(bullet).gameObject;
                    tempBullet2.transform.position = transform.parent.Find("Barrel2").position;
                    tempBullet2.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed + playerSpeed;
                    return true;
                }
                break;
        }
        if (Time.time > lastShotTime + 1/firerate && !isInGUI)
        {
            lastShotTime = Time.time;
            GameObject tempBullet = Instantiate(bullet).gameObject;
            tempBullet.transform.position = position;
            tempBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed + playerSpeed;
            return true;
        }
        return false;
    }
}
