using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlanetLock : MonoBehaviour
{
    public TreeEntry planetLockedItem;
    public string planetName;
    private LevelTree levelTree;

    private void Start()
    {
        levelTree = FindObjectOfType<LevelTree>();
    }

    public void unlock()
    {
        planetLockedItem.lockedByPlanet = false;
        levelTree.unlockPlanetItem(planetLockedItem.gameObject);
        Destroy(gameObject);
    }
}
