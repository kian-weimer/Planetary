using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBackgroundY : MonoBehaviour
{
    public GameObject leftMost;
    public GameObject rightMost;
    private BoxCollider2D boxCollide;
    // Start is called before the first frame update
    void Start()
    {
        boxCollide = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //move from top to bottom
        if(FindObjectOfType<Player>().transform.position.y < transform.position.y - boxCollide.size.y)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - 2f * boxCollide.size.y);
        }

        //move from bottom to top
        if (FindObjectOfType<Player>().transform.position.y > transform.position.y + boxCollide.size.y)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 2f * boxCollide.size.y);
        }
    }
}
