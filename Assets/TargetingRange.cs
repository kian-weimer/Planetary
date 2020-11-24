using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingRange : MonoBehaviour
{
    public EnemyController EC;
    private ArrayList targets = new ArrayList();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Enemy" && EC.CompareTag("Friendly")) {
            EC.inTargetingRange = true;
            EC.target = collision.gameObject;
            targets.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && EC.CompareTag("Enemy") || collision.gameObject.tag == "Enemy" && EC.CompareTag("Friendly"))
        {
            targets.Remove(collision.gameObject);
            if (targets.Count > 0)
            {
                EC.target = (GameObject)targets[targets.Count - 1];
            }
            else
            {
                if (EC.CompareTag("Friendly") && EC.friendlyThatFollowsPlayer)
                {
                    EC.target = EC.player;
                }
                else
                {
                    EC.inTargetingRange = false;
                    EC.target = null;
                }
            }
        }
    }
}
