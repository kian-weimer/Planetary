using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetComboList : MonoBehaviour
{
    public List<PlanetCombo> planetComboList;
    public Dictionary<(string, string, string), PlanetCombo> planetComboDict;

    private void Start()
    {
        planetComboDict = new Dictionary<(string, string, string), PlanetCombo>();
        foreach (PlanetCombo combo in planetComboList)
        {
            // covers all possible orders
            planetComboDict.Add((combo.item1.name, combo.item2.name, combo.item3.name), combo);
            planetComboDict.Add((combo.item1.name, combo.item3.name, combo.item2.name), combo);
            planetComboDict.Add((combo.item2.name, combo.item1.name, combo.item3.name), combo);
            planetComboDict.Add((combo.item2.name, combo.item3.name, combo.item1.name), combo);
            planetComboDict.Add((combo.item3.name, combo.item1.name, combo.item2.name), combo);
            planetComboDict.Add((combo.item3.name, combo.item2.name, combo.item1.name), combo);
        }
    }

    public PlanetCombo Combo(PlanetResource resource1, PlanetResource resource2, PlanetResource resource3)
    {
        if (resource1.resource == null || resource2.resource == null || resource3.resource == null)
        {
            return null;
        }

        PlanetCombo comboResult;
        planetComboDict.TryGetValue((resource1.resource.name.Replace("(Clone)", ""), 
            resource2.resource.name.Replace("(Clone)", ""), 
            resource3.resource.name.Replace("(Clone)", "")), out comboResult);
        return comboResult;
    }
}
