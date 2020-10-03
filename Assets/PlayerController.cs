using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float thrust = 1.0f;
    public float turnSpeed = 1;
    public float maxSpeed = 70f;
  
    [Range(0, 1f)]
    public float landingSpeed = 0.4f;
    [Range(0, 1f)]
    public float breakEffectiveness = 0.2f;
    [Range(0, 0.1f)]
    public float drag = 0.01f;
    public float angularDrag = 0.1f;
    public float stoppingPoint = 0.5f;
    public Rigidbody2D rb;

    public planetGenerator PG;
    public Vector2 gridPosition;
    public Vector2 gridsInView;
    // Start is called before the first frame update
    void Start()
    {
        gridPosition = PG.GetGridPosition(transform.position);
        ArrayList l = getGridsInView();
        for (int i = 0; i < 9; i++)
        {
            PG.InstantiateGrid((Vector2)l[i]);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PG.GetGridPosition(transform.position) != gridPosition)
        {
            ArrayList oldGrids = getGridsInView();
            gridPosition = PG.GetGridPosition(transform.position);
            ArrayList newGrids = getGridsInView();
            for (int i = 0; i < 9; i++)
            {
                // grid should be loaded
                if (!oldGrids.Contains(newGrids[i]))
                {
                    PG.InstantiateGrid((Vector2)newGrids[i]);
                }

                // grid should be unloaded
                if (!newGrids.Contains(oldGrids[i]))
                {
                    PG.DestroyGrid((Vector2)oldGrids[i]);
                }
            }
        }

        if (Input.GetKey("w") && rb.velocity.magnitude < maxSpeed)
        {
            //rb.velocity += new Vector2(0, thrust);
            Vector2 direction = new Vector2(Mathf.Cos(((rb.rotation+90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation + 90) * Mathf.PI) / 180));
            rb.velocity += direction * thrust;
            //rb.velocity += new Vector2(0, thrust);
        }
        else if (Input.GetKey("s"))
        {
            if (Input.GetKey(KeyCode.LeftShift)) {
                Vector2 direction = new Vector2(Mathf.Cos(((rb.rotation + 90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation + 90) * Mathf.PI) / 180));
                rb.velocity = -1 * direction * landingSpeed;
            }
            rb.velocity = rb.velocity * (1 - breakEffectiveness);
        }
        else
        {
            if (rb.velocity.magnitude > 1) {
                //Vector2 direction = new Vector2(Mathf.Cos(((rb.rotation - 90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation - 90) * Mathf.PI) / 180));
                rb.velocity = rb.velocity * (1-drag);
            }
            else if (rb.velocity.magnitude > 0)
            {
                rb.velocity = new Vector2(0,0);
            }
        }

        if (Input.GetKey("a"))
        {
            rb.angularVelocity += turnSpeed;
        }
        else if (Input.GetKey("d"))
        {
            rb.angularVelocity -= turnSpeed;
        }
        else
        {
            rb.angularVelocity -= rb.angularVelocity * angularDrag;
        }
    }

    ArrayList getGridsInView()
    {
        ArrayList grids = new ArrayList();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)// Y GRID POSITION REPORTS 1 VALUES GREATER THAN ACTUAL...
            {
                grids.Add(new Vector2(gridPosition.x + x, gridPosition.y + y));
            }
        }
       
        return grids;
        
    }
}
