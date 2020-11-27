﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringRange : MonoBehaviour
{
    public EnemyController EC;
    private ArrayList targets = new ArrayList();
    public bool isBomber = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBomber)
        {
            if (collision.gameObject.tag == "Planet" && EC.CompareTag("Enemy"))
            {
                EC.inFiringRange = true;
                EC.target = collision.gameObject;
                targets.Add(collision.gameObject);
            }
        }
        else{
            if (collision.gameObject.tag == "Player" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Friendly" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Enemy" && EC.CompareTag("Friendly"))
            {
                EC.inFiringRange = true;
                EC.target = collision.gameObject;
                targets.Add(collision.gameObject);
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
                EC.inFiringRange = false;
                EC.target = null;
            }
        }
    }
}
