﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingRange : MonoBehaviour
{
    public EnemyController EC;
    public bool isBomber = false;
    private ArrayList targets = new ArrayList();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBomber)
        {
            if (collision.gameObject.tag == "Planet" && EC.CompareTag("Enemy"))
            {
                EC.inTargetingRange = true;
                EC.target = collision.gameObject;
                targets.Add(collision.gameObject);
                Debug.Log("Planettt!!!L!!L!");
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Friendly" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Enemy" && EC.CompareTag("Friendly"))
            {
                EC.inTargetingRange = true;
                EC.target = collision.gameObject;
                targets.Add(collision.gameObject);
                Debug.Log("HERE!!!L!!L!");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Friendly" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Enemy" && EC.CompareTag("Friendly") || collision.gameObject.tag == "Planet" && EC.CompareTag("Enemy"))
        {
            targets.Remove(collision.gameObject);
            if (targets.Count > 0)
            {
                EC.target = (GameObject)targets[targets.Count - 1];
            }
            else
            {
                if (EC.CompareTag("Friendly") && EC.friendlyThatFollowsPlayer)
                {
                    EC.target = EC.player;
                }
                else
                {
                    EC.inTargetingRange = false;
                    EC.target = null;
                }
            }
        }
    }
}
