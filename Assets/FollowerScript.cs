using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour
{
    public float distanceAwaySpawn;
    public bool isMoving;
    private Player player;
    private float coolDownTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        float degree = Random.Range(0f, 360f);
        float xOffset = Mathf.Cos(degree * Mathf.PI / 180) * distanceAwaySpawn;
        float yOffset = Mathf.Sin(degree * Mathf.PI / 180) * distanceAwaySpawn;

        gameObject.transform.position = new Vector2(player.transform.position.x + xOffset, player.transform.position.y + yOffset);
        FindObjectOfType<canvas>().broadcast("use the x key to toggle him following you");
        GetComponent<EnemyController>().maxSpeed = FindObjectOfType<PlayerController>().maxSpeed * .95f;

    }

    // Update is called once per frame
    void Update()
    {
        if(coolDownTime <= 0)
        {
            if (Input.GetKey("x") && GetComponent<EnemyController>().inTargetingRange)
            {
                coolDownTime = .5f;
                if (isMoving)
                {
                    GetComponent<EnemyController>().maxSpeed = 0;
                    GetComponent<EnemyController>().speed = 0;
                    isMoving = false;
                }
                else
                {
                    isMoving = true;
                    GetComponent<EnemyController>().maxSpeed = player.GetComponent<PlayerController>().maxSpeed * .95f;
                }
            }
        }
        else
        {
            coolDownTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HomeCircle")
        {
            GetComponent<EnemyController>().spawnExp(10);
            FindObjectOfType<Money>().addMoney(750);
            Destroy(gameObject);
        }
    }
}
