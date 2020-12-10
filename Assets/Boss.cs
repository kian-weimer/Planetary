using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public int bossNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "SampleScene")
        {
            FindObjectOfType<GameManager>().map.addBoss(gameObject, bossNumber);
        }
        
    }

    public void died()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene")
        {
            FindObjectOfType<GameManager>().map.removeBoss(gameObject, bossNumber);
        }
    }
}
