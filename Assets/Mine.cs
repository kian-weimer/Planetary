using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float maxWaitTime;
    private float waitTime;
    public float blastRadius;
    public GameObject explosion;
    private bool hasWaited;
    public bool isExploding;

    // Start is called before the first frame update
    void Start()
    {
        waitTime = maxWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(waitTime >= 0)
        {
            waitTime -= Time.deltaTime;
            hasWaited = true;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(waitTime <= 0 && hasWaited)
        {
            damageSurroundings();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (waitTime <= 0 && hasWaited && (collision.gameObject.tag == "Planet" || collision.gameObject.tag == "Player"))
        {
            damageSurroundings();
        }
    }

    public void damageSurroundings()
    {
        isExploding = true;
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(gameObject.transform.position, blastRadius);

        foreach (Collider2D collider in collidersHit)
        {
            if (collider.gameObject.tag == "Mine")
            {
                if (!collider.gameObject.GetComponent<Mine>().isExploding)
                {
                    collider.gameObject.GetComponent<Mine>().damageSurroundings();
                }
            }
            if (collider.gameObject.tag == "Player")
            {
                collider.gameObject.GetComponent<Player>().loseHealth(50);
            }
            if (collider.gameObject.tag == "Enemy")
            {
                collider.gameObject.GetComponent<EnemyController>().damage(50);
            }
            if (collider.gameObject.tag == "Planet")
            {
                if (!collider.gameObject.GetComponent<Planet>().isDestroyedByMine())
                {
                    collider.gameObject.GetComponent<Planet>().destroy(Player.doubleResource);
                }
            }
            
        }
        GameObject exp = Instantiate(explosion);
        exp.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }
}
