using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetComboList : MonoBehaviour
{
    public List<PlanetCombo> planetComboList;
    public Dictionary<(rsrce, rsrce, rsrce), PlanetCombo> planetComboDict;

    private void Start()
    {
        planetComboDict = new Dictionary<(rsrce, rsrce, rsrce), PlanetCombo>();
        foreach (PlanetCombo combo in planetComboList)
        {
            planetComboDict.Add((combo.item1, combo.item2, combo.item3), combo);
        }
    }

    public PlanetCombo Combo(PlanetResource resource1, PlanetResource resource2, PlanetResource resource3)
    {
        PlanetCombo comboResult;
        planetComboDict.TryGetValue((resource1.resource.GetComponent<rsrce>(), 
            resource2.resource.GetComponent<rsrce>(), 
            resource3.resource.GetComponent<rsrce>()), out comboResult);
        return comboResult;
    }
}
