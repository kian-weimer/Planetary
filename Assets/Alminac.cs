using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;




public class Alminac : MonoBehaviour
{
    public Dictionary<string, bool> discoveredList;
    public GameObject AlminacEntryObject;
    public List<GameObject> AlminacEntryObjectInGame;
    public GameObject window;
    int index = 0;

    public void AddEntry(Sprite planet, string item1, string item2, string item3)
    {
        Debug.Log("addingAlminacEntry");
        Debug.Log(planet);
        Debug.Log(planet.name);
        if (discoveredList == null)
        {
            discoveredList = new Dictionary<string, bool>();
            AlminacEntryObjectInGame = new List<GameObject>();
        }
        

        // will throw an error if we forgot to add the planet to the alminac
        if (!discoveredList.ContainsKey(planet.name))
        {
            discoveredList[planet.name] = true;
            GameObject entry = Instantiate(AlminacEntryObject);
            entry.transform.parent = window.transform;
            entry.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -100 * index, 0);
            index++;
            
            entry.transform.Find("Name").gameObject.GetComponent<Text>().text = Regex.Replace(planet.name, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
            entry.transform.Find("Image").gameObject.GetComponent<UnityEngine.UI.Image>().sprite = planet;
            entry.transform.Find("Item1").gameObject.GetComponent<Text>().text = Regex.Replace(item1, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
            entry.transform.Find("Item2").gameObject.GetComponent<Text>().text = Regex.Replace(item2, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
            entry.transform.Find("Item3").gameObject.GetComponent<Text>().text = Regex.Replace(item3, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }
    }
}

/*
 * public class Alminac : MonoBehaviour
{
    public List<AlminacEntry> entrys;
    public Dictionary<string, AlminacEntry> discoveredList;
    public GameObject AlminacEntryObject;
    public List<GameObject> AlminacEntryObjectInGame;
    public GameObject window;
    int index = 0;


    private void Start()
    {
        foreach (AlminacEntry entry in entrys)
        {
            discoveredList[entry.name] = entry;
        }
    }
    public void AddEntry(GameObject planet, string item1, string item2, string item3)
    {
        // will throw an error if we forgot to add the planet to the alminac
        if (!discoveredList[planet.name].discovered)
        {
            AlminacEntry info = discoveredList[planet.name];
            GameObject entry = Instantiate(AlminacEntryObject);
            entry.transform.parent = window.transform;
            entry.transform.position = new Vector3(0, -100 * index, 0);
            index++;

            entry.transform.Find("Name").gameObject.GetComponent<Text>().text = info.name;
            entry.transform.Find("Image").gameObject.GetComponent<UnityEngine.UI.Image>().sprite = info.icon;
            entry.transform.Find("Item1").gameObject.GetComponent<Text>().text = item1;
            entry.transform.Find("Item2").gameObject.GetComponent<Text>().text = item2;
            entry.transform.Find("Item3").gameObject.GetComponent<Text>().text = item3;
        }
    }
}

    */