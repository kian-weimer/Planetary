﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float thrust = 1.0f;
    public float speed = 0f;
    public float turnSpeed = 1;
    public float maxSpeed = 70f;
  
    [Range(0, 3f)]
    public float landingSpeed = 1f;
    [Range(0, 1f)]
    public float breakEffectiveness = 0.2f;
    [Range(0, 0.1f)]
    public float drag = 0.01f;
    public float angularDrag = 0.1f;
    public float stoppingPoint = 0.5f;
    public Rigidbody2D rb;

    [Range(0, .5f)]
    public float driftPercentage = 0;

    public planetGenerator PG;
    public Vector2 gridPosition;
    public Vector2 gridsInView;
    public Vector2 bbVel = new Vector2(1,1);
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
        bool hasGas = false;
        if ((Input.GetKey("w") && speed < maxSpeed || Input.GetKey("s")) && !gameObject.GetComponent<Player>().isHome)
        {
            hasGas = transform.GetComponent<Player>().ConsumeGas(Input.GetKey("s"));
        }
    
        if (Input.GetKey("w") && speed < maxSpeed && (hasGas || gameObject.GetComponent<Player>().isHome))
        {

            speed += thrust;
            //rb.velocity += new Vector2(0, thrust);
            Vector2 direction = new Vector2(Mathf.Cos(((rb.rotation + 90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation + 90) * Mathf.PI) / 180));

            rb.velocity = direction * speed + rb.velocity * driftPercentage;
            //rb.velocity += new Vector2(0, thrust);
            rb.velocity *= bbVel;

        }
        else if (Input.GetKey("s") && (hasGas || gameObject.GetComponent<Player>().isHome))
        {

            Vector2 direction = new Vector2(Mathf.Cos(((rb.rotation + 90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation + 90) * Mathf.PI) / 180));
            if (Input.GetKey(KeyCode.LeftShift) && speed < stoppingPoint)
            {
                rb.velocity = -1 * direction * landingSpeed;
            }
            else
            {
                speed = speed * (1 - breakEffectiveness);
                rb.velocity = direction * speed + rb.velocity * driftPercentage;
                // rb.velocity = rb.velocity * (1 - breakEffectiveness);
            }
            rb.velocity *= bbVel;

        }
        else
        {
            if (rb.velocity.magnitude > stoppingPoint) {
                //Vector2 direction = new Vector2(Mathf.Cos(((rb.rotation - 90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation - 90) * Mathf.PI) / 180));
                speed = speed * (1 - drag);
                Vector2 direction = new Vector2(Mathf.Cos(((rb.rotation + 90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation + 90) * Mathf.PI) / 180));
                rb.velocity = direction * speed + rb.velocity * driftPercentage;
                rb.velocity *= bbVel;
                // rb.velocity = rb.velocity * (1-drag);
            }
            else if (rb.velocity.magnitude > 0)
            {
                speed = 0;
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

    public void bounceBack()
    {
        bbVel = new Vector2(-0.3f, -0.3f) * bbVel;
        Invoke("StopBounceBack", 0.6f);
    }

    public void StopBounceBack()
    {
        bbVel = new Vector2(1, 1);
        speed = 0;
    }
}
