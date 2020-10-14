using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float maxLifeTime = 20;
    private float timeLeftTillDestroy;
    // Start is called before the first frame update
    void Start()
    {
        timeLeftTillDestroy = maxLifeTime;
        float angle = ((transform.parent.eulerAngles.z-90) * Mathf.PI) / 180;// - ( 45 * Mathf.PI) / 180;
        //float angle = (transform.rotation.z * Mathf.PI) / 180;
        GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))* Random.Range(5f, 10f); //new Vector2(0, -Random.Range(5, 30));
        GetComponent<Rigidbody2D>().angularVelocity = 720;
    }

    void Update()
    {
        if (timeLeftTillDestroy > 0)
        {
            timeLeftTillDestroy -= Time.deltaTime;
        }
        if(timeLeftTillDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().loseHealth(10);
            Destroy(gameObject);
        }
    }

    public void move()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, -Random.Range(5, 30));
    }
}
