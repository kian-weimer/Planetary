using System.Collections;
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
            }
        }
        else
        {
            if (collision.gameObject.tag == "Player" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Friendly" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Enemy" && EC.CompareTag("Friendly"))
            {
                EC.inTargetingRange = true;
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

                GameObject stoppingRange = transform.parent.gameObject.transform.Find("StoppingRange").gameObject;
                Collider2D[] collidersHit = Physics2D.OverlapCircleAll(gameObject.transform.position, stoppingRange.GetComponent<CircleCollider2D>().radius);
                EC.inStoppingRange = false;
                foreach (Collider2D collider in collidersHit)
                {
                    if (collider.gameObject.name == EC.target.name)
                    {
                        EC.inStoppingRange = true;
                    }
                }
            }
            else
            {
                if (EC.CompareTag("Friendly") && EC.friendlyThatFollowsPlayer)
                {
                    EC.target = EC.player;
                }
                else
                {
                    EC.inStoppingRange = false;
                    EC.inTargetingRange = false;
                    EC.target = null;
                }
            }
        }
    }
}
