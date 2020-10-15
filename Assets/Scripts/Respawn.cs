using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public float waitTime;
    public GameObject spawnLocation;
    // Start is called before the first frame update
    public void playerRespawn()
    {
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<Player>().health = 1;
        GetComponent<Player>().transform.position = spawnLocation.transform.position;
    }
}
