using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HomePlanetInfo
{
    public string name;
    public bool hasShield = false;
    public float shieldHealth;
    public float health;
    public int comboIndex = -1;

    //[SerializeField]
    //public List<ProductionItem> productionItems;


    public HomePlanetInfo(string name)
    {
        this.name = name;
    }
}
