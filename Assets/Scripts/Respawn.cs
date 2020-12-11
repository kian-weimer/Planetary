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
        EnemyController.paused = true;
        if (!isRespawning)
        {
            transform.Find("PlayerTrail").gameObject.SetActive(false);
            if (FindObjectOfType<OxygenEvent>() != null)
            {
                FindObjectOfType<OxygenEvent>().endEvent();
            }
            StartCoroutine(wait(isGasDeath));
        }
    }

    IEnumerator wait(bool isGas)
    {
        isRespawning = true;

        if (!isGas)
        {
            FindObjectOfType<AudioManager>().PlayIfNotPlaying("PlayerExplosion");
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
            
            if (EnemyInvasionManager.invasionOccuring)
            {
                GetComponent<Player>().healthBarEnd.transform.localScale = new Vector3(1, GetComponent<Player>().healthBarEnd.transform.localScale.y, GetComponent<Player>().healthBarEnd.transform.localScale.z);
                GetComponent<Player>().healthBar.transform.localScale = new Vector3(1, GetComponent<Player>().healthBar.transform.localScale.y, GetComponent<Player>().healthBar.transform.localScale.z);
                GetComponent<Player>().health = GetComponent<Player>().maxHealth;
                GetComponent<Player>().gasBarEnd.transform.localScale = new Vector3(1, GetComponent<Player>().gasBarEnd.transform.localScale.y, GetComponent<Player>().gasBarEnd.transform.localScale.z);
                GetComponent<Player>().gasBar.transform.localScale = new Vector3(1, GetComponent<Player>().gasBar.transform.localScale.y, GetComponent<Player>().gasBar.transform.localScale.z);
                GetComponent<Player>().gas = GetComponent<Player>().maxGas;
            }
            else
            {
                GetComponent<Player>().health = 1;
            }
            
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
        EnemyController.paused = false;
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
    }

    public void warpHome()
    {
        GetComponent<Player>().transform.position = spawnLocation.transform.position;
    }
}
