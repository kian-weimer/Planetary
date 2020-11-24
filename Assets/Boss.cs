using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int bossNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<GameManager>().map.addBoss(gameObject, bossNumber);
    }

    public void died()
    {
        FindObjectOfType<GameManager>().map.removeBoss(gameObject, bossNumber);
    }
}
