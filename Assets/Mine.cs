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

    public void damageSurroundings()
    {
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(gameObject.transform.position, blastRadius);

        foreach (Collider2D collider in collidersHit)
        {
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
                collider.gameObject.GetComponent<Planet>().destroy(Player.doubleResource);
            }
        }
        GameObject exp = Instantiate(explosion);
        exp.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }
}
