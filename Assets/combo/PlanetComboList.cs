﻿
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlanetComboList : MonoBehaviour
{
    public List<PlanetCombo> planetComboList;
    public GameObject planetTreeLocks;
    public Dictionary<(string, string, string), PlanetCombo> planetComboDict;
    public Alminac alminac;
    public QuestSystem questSystem;

    private void Start()
    {
        if (planetComboDict == null)
        {
            setup();
        }
    }

    private void setup()
    {
        planetComboDict = new Dictionary<(string, string, string), PlanetCombo>();
        int i = 0;
        foreach (PlanetCombo combo in planetComboList)
        {
            // covers all possible orders
            combo.index = i;
            planetComboDict[(combo.item1.name, combo.item2.name, combo.item3.name)] = combo;
            planetComboDict[(combo.item1.name, combo.item3.name, combo.item2.name)] = combo;
            planetComboDict[(combo.item2.name, combo.item1.name, combo.item3.name)] = combo;
            planetComboDict[(combo.item2.name, combo.item3.name, combo.item1.name)] = combo;
            planetComboDict[(combo.item3.name, combo.item1.name, combo.item2.name)] = combo;
            planetComboDict[(combo.item3.name, combo.item2.name, combo.item1.name)] = combo;
            i++;
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
        if (comboResult != null)
        {
            questSystem.updateQuestsPlanet(comboResult.planet.name);
            alminac.AddEntry(comboResult.planet.GetComponent<SpriteRenderer>().sprite, resource1.resource.name.Replace("(Clone)", ""),
                resource2.resource.name.Replace("(Clone)", ""), resource3.resource.name.Replace("(Clone)", ""));

            for (int i = 0; i < planetTreeLocks.transform.childCount; i++)
            {
                if (comboResult.planet.name.Contains(planetTreeLocks.transform.GetChild(i).GetComponent<TreePlanetLock>().planetName)) {
                    planetTreeLocks.transform.GetChild(i).GetComponent<TreePlanetLock>().unlock();
                }
            }

            if (comboResult.planet.name.Contains("EndGame"))
            {
                FindObjectOfType<GameManager>().BM.Broadcast("congratulations, you have found the secret to the universe! You can continue playing if you like, but your quest is fufilled.");
            }
        }

        return comboResult;
    }

    // does not do any quest, alminac, or level tree updating
    // this is to only be used for loading and therefore the other things will already be loaded in
    public PlanetCombo ComboFromIndex(int index)
    {
        if (planetComboDict == null)
        {
            setup();
        }
        PlanetCombo comboResult;
        comboResult = planetComboList.ElementAt(index);
        return comboResult;
    }
}
