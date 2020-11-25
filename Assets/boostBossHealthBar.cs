using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boostBossHealthBar : MonoBehaviour
{

    public BossSprintAtPlayer boostBoss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(boostBoss.health / boostBoss.maxHealth, transform.localScale.y, 1);
    }
}
