using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMovement : MonoBehaviour
{
    private int movementPercent;
    private Rigidbody2D rb;
    private float turnSpeed;
    public tutorialGameManager tutGameManager;
    public bool canBegin = false;
    // Start is called before the first frame update
    void Start()
    {
        movementPercent = 0;
        turnSpeed = gameObject.GetComponent<PlayerController>().turnSpeed;
        rb = gameObject.GetComponent<Rigidbody2D>();
        gameObject.GetComponent<PlayerController>().canMove = false;
        gameObject.GetComponent<Player>().canShoot = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canBegin)
        {
            if (Input.GetKey("a") && movementPercent <= 1)
            {
                rb.angularVelocity += turnSpeed;
                if (movementPercent == 0)
                {
                    movementPercent += 1;
                }
            }
            else if (Input.GetKey("d") && movementPercent <= 1)
            {
                rb.angularVelocity -= turnSpeed;

                if(movementPercent == 1)
                {
                    movementPercent += 1;
                }
            }

            if (movementPercent == 2)
            {
                tutGameManager.updateMessageText("Move forward using w");
                gameObject.GetComponent<PlayerController>().canMove = true;
            }

            if (Input.GetKey("w") && movementPercent == 2)
            {
                if(movementPercent == 2)
                {
                    movementPercent += 1;
                }
                tutGameManager.updateMessageText("To move backwards hold down", "shift and s");
            }
            if (Input.GetKey("s") && Input.GetKey(KeyCode.LeftShift) && movementPercent == 3)
            {
                if(movementPercent == 3)
                {
                    movementPercent += 1;
                }
                tutGameManager.updateMessageText("You can zoom your veiw in", "and out using the scroll wheel");
            }
            if(Input.GetAxis("Mouse ScrollWheel") != 0 && movementPercent == 4)
            {
                tutGameManager.updateMessageText();
            }
        }
    }
}
