using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;
    // Start is called before the first frame update
    public float despawnTime = 3;

    private float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (Time.time >= spawnTime + despawnTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            collision.gameObject.GetComponent<Planet>().health = collision.gameObject.GetComponent<Planet>().health - 10;

            if (collision.gameObject.GetComponent<Planet>().health <= 0)
            {
                if (!collision.gameObject.GetComponent<Planet>().destroyed)
                {
                    collision.gameObject.GetComponent<Planet>().destroyed = true;
                    collision.gameObject.GetComponent<Planet>().destroy();
                }
            }
        }

        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Popup" && collision.gameObject.tag != "HomeCircle")
        {
            GameObject exp = Instantiate(explosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            Destroy(gameObject);
        }
    }
}