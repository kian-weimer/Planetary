
using System.Collections;
using System.Collections.Generic;
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
        planetComboDict = new Dictionary<(string, string, string), PlanetCombo>();
        foreach (PlanetCombo combo in planetComboList)
        {
            // covers all possible orders
            
            planetComboDict[(combo.item1.name, combo.item2.name, combo.item3.name)] = combo;
            planetComboDict[(combo.item1.name, combo.item3.name, combo.item2.name)] = combo;
            planetComboDict[(combo.item2.name, combo.item1.name, combo.item3.name)] = combo;
            planetComboDict[(combo.item2.name, combo.item3.name, combo.item1.name)] = combo;
            planetComboDict[(combo.item3.name, combo.item1.name, combo.item2.name)] = combo;
            planetComboDict[(combo.item3.name, combo.item2.name, combo.item1.name)] = combo;
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
        Debug.Log(comboResult);
        if (comboResult != null)
        {
            questSystem.updateQuestsPlanet(comboResult.planet.name);
            alminac.AddEntry(comboResult.planet.GetComponent<SpriteRenderer>().sprite, resource1.resource.name.Replace("(Clone)", ""),
                resource2.resource.name.Replace("(Clone)", ""), resource3.resource.name.Replace("(Clone)", ""));

            if (SceneManager.GetActiveScene().name == "Tutorial")
            {
                try
                {
                    if(FindObjectOfType<TutorialResourceCollection>().isWaitingOnBasePlanet && resource1.resource.GetComponent<rsrce>().nameOfResource == "Oxygen" &&
                        resource2.resource.GetComponent<rsrce>().nameOfResource == "Oxygen" && resource3.resource.GetComponent<rsrce>().nameOfResource == "Oxygen")
                    {
                        FindObjectOfType<TutorialResourceCollection>().createdBasePlanet();
                    }
                    Debug.Log(comboResult.planet.name);
                    if (comboResult.planet.name == "FoodPlanet")
                    {
                        FindObjectOfType<TutorialResourceCollection>().createdComboPlanet();
                    }
                }
                catch
                {
                    
                }
            }

            for (int i = 0; i < planetTreeLocks.transform.childCount; i++)
            {
                if (comboResult.planet.name.Contains(planetTreeLocks.transform.GetChild(i).GetComponent<TreePlanetLock>().planetName)) {
                    planetTreeLocks.transform.GetChild(i).GetComponent<TreePlanetLock>().unlock();
                }
            }

            if (comboResult.planet.name.Contains("EndGame"))
            {
                FindObjectOfType<canvas>().transform.Find("EndGamePrompt").gameObject.SetActive(true);
            }
        }

        return comboResult;
    }
}
