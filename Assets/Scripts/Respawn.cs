using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public float waitTime;
    public GameObject spawnLocation;
    private bool isRespawning = false;
    public GameObject explosion;
    // Start is called before the first frame update
    public void playerRespawn(bool isGasDeath)
    {
        if (!isRespawning)
        {
            transform.Find("PlayerTrail").gameObject.SetActive(false);
            StartCoroutine(wait(isGasDeath));
        }
    }

    IEnumerator wait(bool isGas)
    {
        isRespawning = true;

        if (!isGas)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            GameObject explosionObject = Instantiate(explosion);
            explosionObject.transform.position = transform.position;
        }

        yield return new WaitForSeconds(waitTime);
        if (!isGas && GetComponent<Player>().isHome)
        {
            yield return new WaitForSeconds(waitTime);
            if (FindObjectOfType<PauseMenu>() != null)
            {
                FindObjectOfType<PauseMenu>().togglePauseMenu();
                if (FindObjectOfType<PauseMenu>() != null)
                {
                    FindObjectOfType<PauseMenu>().togglePauseMenu();
                }
            }
            GetComponent<Player>().health = 1;
            GetComponent<Player>().gas = GetComponent<Player>().maxGas / 12;
            GetComponent<Player>().transform.position = spawnLocation.transform.position;
            GetComponent<PlayerController>().canMove = true;
            GetComponent<PlayerController>().hasRespawned = false;
            List<GameObject> playerInventory = FindObjectOfType<Inventory>().slots;
            foreach (GameObject playerItem in playerInventory)
            {
                if (playerItem.GetComponent<InventorySlot>().item != null)
                {
                    FindObjectOfType<ResourceInventory>().checkForItemAndRemove(playerItem.GetComponent<InventorySlot>().item.gameObject.GetComponent<rsrce>().nameOfResource);
                    Destroy(playerItem.GetComponent<InventorySlot>().item);
                    Destroy(playerItem.GetComponent<InventorySlot>().icon);
                }
            }
            FindObjectOfType<Inventory>().isFull = false;
            transform.Find("PlayerTrail").gameObject.SetActive(true);
            isRespawning = false;
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
            
        }
        else if (GetComponent<Player>().isHome == false)
        {
            if (FindObjectOfType<PauseMenu>() != null)
            {
                FindObjectOfType<PauseMenu>().togglePauseMenu();
                if (FindObjectOfType<PauseMenu>() != null)
                {
                    FindObjectOfType<PauseMenu>().togglePauseMenu();
                }
            }
            GetComponent<Player>().health = 1;
            GetComponent<Player>().gas = GetComponent<Player>().maxGas / 12;
            GetComponent<Player>().transform.position = spawnLocation.transform.position;
            GetComponent<PlayerController>().canMove = true;
            GetComponent<PlayerController>().hasRespawned = false;
            List<GameObject> playerInventory = FindObjectOfType<Inventory>().slots;
            foreach (GameObject playerItem in playerInventory)
            {
                if (playerItem.GetComponent<InventorySlot>().item != null)
                {
                    FindObjectOfType<ResourceInventory>().checkForItemAndRemove(playerItem.GetComponent<InventorySlot>().item.gameObject.GetComponent<rsrce>().nameOfResource);
                    Destroy(playerItem.GetComponent<InventorySlot>().item);
                    Destroy(playerItem.GetComponent<InventorySlot>().icon);
                }
            }
            FindObjectOfType<Inventory>().isFull = false;
            transform.Find("PlayerTrail").gameObject.SetActive(true);
            
        }
        else
        {
            transform.Find("PlayerTrail").gameObject.SetActive(true);
            GetComponent<PlayerController>().canMove = true;
        }
        isRespawning = false;
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

    public void warpHome()
    {
        GetComponent<Player>().transform.position = spawnLocation.transform.position;
    }
}
