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
        transform.Find("PlayerTrail").gameObject.SetActive(false);
        StartCoroutine(wait());
        
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(waitTime);
        GetComponent<Player>().health = 1;
        GetComponent<Player>().gas = GetComponent<Player>().maxGas/12;
        GetComponent<Player>().transform.position = spawnLocation.transform.position;
        GetComponent<PlayerController>().canMove = true;
        GetComponent<PlayerController>().hasRespawned = false;
        List<GameObject> playerInventory = FindObjectOfType<Inventory>().slots;
        foreach (GameObject playerItem in playerInventory)
        {
            if (playerItem.GetComponent<InventorySlot>().item != null)
            {
                Destroy(playerItem.GetComponent<InventorySlot>().item);
                Destroy(playerItem.GetComponent<InventorySlot>().icon);
            }
        }
        FindObjectOfType<Inventory>().isFull = false;
        transform.Find("PlayerTrail").gameObject.SetActive(true);
    }
}
