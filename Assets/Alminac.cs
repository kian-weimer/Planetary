using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

    List<Entry> entrys = new List<Entry>();
    public List<Sprite> sprites;


    public bool AddEntry(Sprite planet, string item1, string item2, string item3)
    {
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

            int i = 0;



            entrys.Add(new Entry(sprites.IndexOf(planet), item1, item2, item3));

            return true;
        }
        return false;
    }

    public void Save()
    {
        FileStream fs = new FileStream("savedAlmanacData.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, entrys);
        fs.Close();
    }

    public void Load()
    {
        List<Entry> tempEntrys = new List<Entry>();

        using (Stream stream = File.Open("savedAlmanacData.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            tempEntrys = (List<Entry>)bformatter.Deserialize(stream);
        }


        foreach(Entry entry in tempEntrys)
        {
            AddEntry(sprites[entry.spriteIndex], entry.item1, entry.item2, entry.item3);
        }
    }

}


[System.Serializable]
public class Entry
{
    SerializeTexture spriteTexture = new SerializeTexture();

    //string spriteName;
    public int spriteIndex;
    public string item1;
    public string item2;
    public string item3;

    public Entry(int spriteIndex, string item1, string item2, string item3)
    {
        //Texture2D tex = planet.texture;
        //spriteTexture.x = tex.width;
        //spriteTexture.y = tex.height;

        // doesnt work because textures are compressed...
        //spriteTexture.bytes = ImageConversion.EncodeToPNG(tex);

        //this.spriteName = spriteName;
        this.spriteIndex = spriteIndex;
        this.item1 = item1;
        this.item2 = item2;
        this.item3 = item3;
    }

    public Sprite GenerateSprite()
    {
        Texture2D tex = new Texture2D(spriteTexture.x, spriteTexture.y);
        ImageConversion.LoadImage(tex, spriteTexture.bytes);
        Sprite mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.one);
        return mySprite;
    }
}

[Serializable]
public class SerializeTexture
{
    [SerializeField]
    public int x;
    [SerializeField]
    public int y;
    [SerializeField]
    public byte[] bytes;
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
