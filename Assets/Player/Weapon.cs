using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Bullet bullet;
    public float firerate; // bullets per second
    public float bulletSpeed;
    public float lastShotTime;
    public bool isInGUI = true;

    public bool Shoot(Vector2 position, Vector2 direction, Vector2 playerSpeed)
    {
        
        if (Time.time > lastShotTime + 1/firerate && !isInGUI)
        {
            lastShotTime = Time.time;
            Bullet tempBullet = Instantiate(bullet);
            tempBullet.transform.position = position;
            tempBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed + playerSpeed;
            return true;
        }
        return false;
    }
}
