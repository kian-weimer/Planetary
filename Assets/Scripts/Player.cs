using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInfo info;

    public int maxHealth; 

    [HideInInspector]
    public int health; // player's health

    public Weapon weapon;

    public Vector3 position;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
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
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            weapon.Shoot(transform.position, new Vector2(Mathf.Cos(((rb.rotation+90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation + 90) * Mathf.PI) / 180)));
        }
    }
}
