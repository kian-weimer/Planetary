using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float thrust = 1.0f;
    public float speed = 0f;
    public float turnSpeed = 1;
    public float maxSpeed = 70f;

    public bool hasWarpSpeed = false;
    public float warpSpeedModifier = 1.5f;
    public float maxWarpSpeed = 100;
    public float warpSpeed;
    public float warpConsumption = 1;
    public float warpRegenRate = 0.1f;
    public float warpFuelModifier = 3f;
    public float warpResumeThreshold = 0.5f;
    private bool canWarp = true;
    public bool boosting = false;
    public GameObject warpSpeedBar;

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

    [HideInInspector]
    public bool canMove;

    public AudioManager audioManager;
    private bool hasGas;
    public bool hasRespawned = false;

    // Start is called before the first frame update
    void Start()
    {
        warpSpeed = maxWarpSpeed;

        if (SceneManager.GetActiveScene().name == "SampleScene") 
        {
            canMove = true;
        }
        

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
        
        if(hasWarpSpeed && Input.GetKey("space") && canWarp && warpSpeed > 0)
        {
            if (boosting == false)
            {
                thrust = thrust * warpSpeedModifier;
                maxSpeed = maxSpeed * warpSpeedModifier;
                transform.GetComponent<Player>().fuelConsumption = transform.GetComponent<Player>().fuelConsumption * warpFuelModifier;
            }
            
            warpSpeed -= warpConsumption;
            warpSpeedBar.GetComponent<RectTransform>().localScale = new Vector3(warpSpeed / maxWarpSpeed,
                warpSpeedBar.GetComponent<RectTransform>().localScale.y,
                warpSpeedBar.GetComponent<RectTransform>().localScale.z);
            boosting = true;
        }
        else if (hasWarpSpeed && boosting)
        {
            canWarp = false;
            boosting = false;
            thrust = thrust / warpSpeedModifier;
            maxSpeed = maxSpeed / warpSpeedModifier;
            transform.GetComponent<Player>().fuelConsumption = transform.GetComponent<Player>().fuelConsumption / warpFuelModifier;
        }
        else if (hasWarpSpeed && warpSpeed < maxWarpSpeed)
        {
            warpSpeed += warpRegenRate;
            warpSpeedBar.GetComponent<RectTransform>().localScale = new Vector3(warpSpeed / maxWarpSpeed,
                warpSpeedBar.GetComponent<RectTransform>().localScale.y,
                warpSpeedBar.GetComponent<RectTransform>().localScale.z);
        }
        else if (hasWarpSpeed && warpSpeed >= warpResumeThreshold * maxWarpSpeed)
        {
            canWarp = true;
        }
        
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
        hasGas = false;
        if ((Input.GetKey("w") && speed < maxSpeed || Input.GetKey("s")) && !gameObject.GetComponent<Player>().isHome && canMove)
        {
            hasGas = transform.GetComponent<Player>().ConsumeGas(Input.GetKey("s")); 
        }
    
        if (Input.GetKey("w") && speed < maxSpeed && (hasGas || gameObject.GetComponent<Player>().isHome) && canMove)
        {

            speed += thrust;
            audioManager.PlayIfNotPlaying("Thrust");
            //rb.velocity += new Vector2(0, thrust);
            Vector2 direction = new Vector2(Mathf.Cos(((rb.rotation + 90) * Mathf.PI) / 180), Mathf.Sin(((rb.rotation + 90) * Mathf.PI) / 180));

            rb.velocity = direction * speed + rb.velocity * driftPercentage;
            //rb.velocity += new Vector2(0, thrust);
            rb.velocity *= bbVel;

        }

        else if (Input.GetKey("s") && (hasGas || gameObject.GetComponent<Player>().isHome) && canMove)
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
            if (!Input.GetKey("w"))
            {
                audioManager.Stop("Thrust");
            }
            
        }

        if (Input.GetKey("a") && canMove)
        {
            rb.angularVelocity += turnSpeed;
        }
        else if (Input.GetKey("d") && canMove)
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
