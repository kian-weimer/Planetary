using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerScript : MonoBehaviour
{
    public float distanceAwaySpawn;
    // Start is called before the first frame update
    void Start()
    {
        float degree = Random.Range(0f, 360f);
        float xOffset = Mathf.Cos(degree * Mathf.PI / 180) * distanceAwaySpawn;
        float yOffset = Mathf.Sin(degree * Mathf.PI / 180) * distanceAwaySpawn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
