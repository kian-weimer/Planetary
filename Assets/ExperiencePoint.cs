using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePoint : MonoBehaviour
{
    Rigidbody2D rb;
    GameObject player;
    bool mobile = false;
    public int value = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>().gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (mobile)
        {
            Vector3 playerRelativePosition = player.transform.position - transform.position;
            float playerDirection = Mathf.Atan(playerRelativePosition.y / playerRelativePosition.x);
            rb.velocity = playerRelativePosition.normalized * 4f;
        }
        else
        {
            rb.velocity *= 0.99f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().addExpPoints(value);
            Destroy(gameObject);
        }
        if (collision.transform.CompareTag("playerRange"))
        {
            mobile = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("playerRange"))
        {
            mobile = false;
        }
    }
}
