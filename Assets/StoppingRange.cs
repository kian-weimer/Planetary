using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoppingRange : MonoBehaviour
{
    public EnemyController EC;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EC.inStoppingRange = true;
        }
        if (EC.target != null)
        {
            if (collision.gameObject.tag == EC.target.tag)
            {
                EC.inStoppingRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EC.inStoppingRange = false;
        }
        if (EC.target != null)
        {
            if (collision.gameObject.tag == EC.target.tag)
            {
                EC.inStoppingRange = false;
            }
        }
    }
}
