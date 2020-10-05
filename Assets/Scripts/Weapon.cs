using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Bullet bullet;
    public float firerate; // bullets per second
    public float bulletSpeed;
    public float lastShotTime;

    public void Shoot(Vector3 position, Vector2 direction)
    {
        if (Time.time > lastShotTime + 1/firerate)
        {
            lastShotTime = Time.time;
            Bullet tempBullet = Instantiate(bullet);
            tempBullet.transform.position = position;
            tempBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
        }
    }
}
