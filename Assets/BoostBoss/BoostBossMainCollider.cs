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

            GameObject exp = Instantiate(collision.gameObject.GetComponent<Bullet>().explosion);
            exp.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z);
            FindObjectOfType<AudioManager>().Play("BulletExplosion");
            Destroy(collision.gameObject);
        }
    }
}
