using System.Collections;
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
            case 3:
                if (Time.time > lastShotTime + 1 / firerate && !isInGUI)
                {
                    lastShotTime = Time.time;
                    GameObject tempBullet = Instantiate(bullet).gameObject;
                    tempBullet.transform.position = position;
                    tempBullet.transform.localScale *= 3;
                    tempBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed + playerSpeed;
                    lastShotTime = Time.time;
                    GameObject tempBullet1 = Instantiate(bullet).gameObject;
                    tempBullet1.transform.position = transform.parent.Find("Barrel1").position;
                    tempBullet1.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed + playerSpeed;
                    tempBullet1.transform.parent = tempBullet.transform;
                    GameObject tempBullet2 = Instantiate(bullet).gameObject;
                    tempBullet2.transform.position = transform.parent.Find("Barrel2").position;
                    tempBullet2.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed + playerSpeed;
                    return true;
                }
                break;
            case 5: // basic shooting
                if (Time.time > lastShotTime + 1 / firerate && !isInGUI)
                {
                    //wider bullet
                    lastShotTime = Time.time;
                    GameObject tempBullet = Instantiate(bullet).gameObject;
                    tempBullet.GetComponent<EnemyBullet>().despawnTime = Random.Range(1f, 6f);
                    tempBullet.transform.position = position;
                    float xVariance = Random.Range(-0.18f, 0.18f);
                    float yVariance = Random.Range(-0.18f, 0.18f);
                    tempBullet.GetComponent<Rigidbody2D>().velocity = (direction.normalized + new Vector2(xVariance, yVariance)) * bulletSpeed + playerSpeed;
                    //narrower bullet
                    lastShotTime = Time.time;
                    GameObject tempBullet2 = Instantiate(bullet).gameObject;
                    tempBullet2.GetComponent<EnemyBullet>().despawnTime = Random.Range(3f, 9f);
                    tempBullet2.transform.position = position;
                    float xVariance2 = Random.Range(-0.08f, 0.08f);
                    float yVariance2 = Random.Range(-0.08f, 0.08f);
                    tempBullet2.GetComponent<Rigidbody2D>().velocity = (direction.normalized + new Vector2(xVariance2, yVariance2)) * bulletSpeed + playerSpeed;
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
