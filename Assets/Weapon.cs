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
        Debug.Log("TICK!" + Time.time + "     " + lastShotTime);
        if (Time.time > lastShotTime + 1/firerate)
        {
            lastShotTime = Time.time;
            Bullet tempBullet = Instantiate(bullet);
            tempBullet.transform.position = position;
            Debug.Log("BANG!");
            tempBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed;
        }
    }
}
