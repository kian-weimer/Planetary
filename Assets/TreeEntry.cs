using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntry : MonoBehaviour
{
    public List<GameObject> children;
    public string action;
    public bool repeatable = false;

    public bool lockedByPlanet = false;
}
