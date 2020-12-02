using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBossMainCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Planet")
        {
            collision.gameObject.GetComponent<Planet>().destroy(false);
        }
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().loseHealth(20);
        }
        if(collision.gameObject.tag == "Bullet")
        {
            transform.parent.gameObject.GetComponent<BossSprintAtPlayer>().health -= collision.gameObject.GetComponent<Bullet>().bulletDamage;
            if(transform.parent.gameObject.GetComponent<BossSprintAtPlayer>().health <= 0)
            {
                transform.parent.GetComponent<BossSprintAtPlayer>().death();
            }
        }
    }
}
