using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetComboList : MonoBehaviour
{
    public List<PlanetCombo> planetComboList;
    public Dictionary<(rsrce, rsrce, rsrce), GameObject> planetComboDict;

    private void Start()
    {
        foreach (PlanetCombo combo in planetComboList)
        {
            Planet comboPlanet = Instantiate(combo.planet);
            //comboPlanet.gameObject.AddComponent<HomePlanet>();

            planetComboDict.Add((combo.item1, combo.item2, combo.item3), comboPlanet.gameObject);
        }
    }
}
