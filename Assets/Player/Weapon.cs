using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public GameObject enemySpawnable;
    public float firerate; // bullets per second
    public float bulletSpeed;
    public float lastShotTime;
    public bool isInGUI = true;
    public int weaponSelect = 1;
    private int count = 0;
    private int spawnCount = 0;

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
            case 6: // basic shooting
                if (Time.time > lastShotTime + 1 / firerate && !isInGUI)
                {
                    if (count < 5)
                    {
                        count++;
                        for (int i = 0; i < 3; i++)
                        {
                            lastShotTime = Time.time;
                            GameObject tempBullet = Instantiate(bullet).gameObject;
                            tempBullet.transform.position = position;
                            //tempBullet.GetComponent<Rigidbody2D>().velocity = (direction.normalized*(1 - i/50) + new Vector2(2.5f - count, -2.5f + count)) * bulletSpeed + playerSpeed;
                            float xVariance = Random.Range(-0.01f, 0.01f);
                            float yVariance = Random.Range(-0.01f, 0.01f);
                            tempBullet.GetComponent<Rigidbody2D>().velocity = (direction.normalized + new Vector2(xVariance, yVariance)) * bulletSpeed + playerSpeed;
                        }
                        
                        return true;
                    }
                    else
                    {
                        lastShotTime += 10000 / firerate;
                        count = 0;
                        spawnCount++;
                        if (spawnCount == 5)
                        {
                            spawnCount = 0;
                            GameObject enemy = Instantiate(enemySpawnable);
                            enemy.transform.position = position;
                            enemy.GetComponent<EnemyController>().inTargetingRange = true;
                        }
                    }
                }
                break;
            case 7:
                if (Time.time > lastShotTime + 1 / firerate && !isInGUI)
                {
                    float angle = 0;
                    for (int i = 0; i < 16; i++)
                    {
                        lastShotTime = Time.time;
                        GameObject tempBullet = Instantiate(bullet).gameObject;
                        tempBullet.transform.position = transform.parent.position;
                        tempBullet.GetComponent<Rigidbody2D>().velocity = (new Vector2(Mathf.Cos(Mathf.Deg2Rad*angle), Mathf.Sin(Mathf.Deg2Rad * angle))) * bulletSpeed;
                        angle += 360 / 16;
                    }

                    return true;
                }
                break;

            case 8:
                lastShotTime = Time.time;
                GameObject tmpBullet = Instantiate(bullet).gameObject;
                tmpBullet.transform.position = position;
                tmpBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * bulletSpeed + playerSpeed;
                return true;
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
