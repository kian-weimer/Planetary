using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    float startTime = 0;
    float despawnTime = 5;
    // Start is called before the first frame update
    void OnEnable()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime > 0 && Time.time - startTime > despawnTime )
        {
            gameObject.SetActive(false);
        }
    }
}
