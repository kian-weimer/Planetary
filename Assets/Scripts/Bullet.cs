using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Planet")
        {
            collision.gameObject.GetComponent<Planet>().health = collision.gameObject.GetComponent<Planet>().health - 10;

            if (collision.gameObject.GetComponent<Planet>().health <= 0)
            {
                collision.gameObject.GetComponent<Planet>().destroy();
            }

        }

        if (collision.gameObject.tag != "Player")
        {
            GameObject exp = Instantiate(explosion);
            exp.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            Destroy(gameObject);
        }
    }
}
