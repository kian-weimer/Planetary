using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingField : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Player"))
        {
            transform.parent.GetComponent<EnemyBullet>().targeting = true;
            transform.parent.GetComponent<Rigidbody2D>().velocity = transform.parent.GetComponent<Rigidbody2D>().velocity * 0.80f;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            transform.parent.GetComponent<EnemyBullet>().targeting = false;
        }
    }
}
