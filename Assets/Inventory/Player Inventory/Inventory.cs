using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> slots;
    public int numberOfInventorySlots = 1;

    public Vector3 origin = new Vector3(80, 80, 0);
    public int spacing = 80;

    public bool isFull = false;

    public GameObject IS;

    public List<GameObject> resources;
    // Start is called before the first frame update
    void Start()
    {
        InitializeInventory();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("NumberOfInventorySlots", numberOfInventorySlots);
        List<string> names = new List<string>();

        foreach(GameObject resource in slots)
        {
            if(resource.GetComponent<InventorySlot>().item != null)
            {
                names.Add(resource.GetComponent<InventorySlot>().item.name.Replace("(Clone)", ""));
            }
        }
        FileStream fs = new FileStream("savedInventory.dat", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, names);
        fs.Close();
    }

    public void Load()
    {
        List<string> names = new List<string>();
        using (Stream stream = File.Open("savedInventory.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            names = (List<string>)bformatter.Deserialize(stream);
        }

        foreach(Transform slot in gameObject.transform)
        {
            if(slot.gameObject.name == "InventorySlot(Clone)"){
                Destroy(slot.gameObject);
            }
        }
        slots.Clear();
        numberOfInventorySlots = PlayerPrefs.GetInt("NumberOfInventorySlots");
        for(int i  = 0; i < numberOfInventorySlots; i++)
        {
            addInventorySlot();
        }

        foreach(string name in names)
        {
            foreach(GameObject resource in resources)
            {
                if(name == resource.name)
                {
                    GameObject resourceToSpawn = Instantiate(resource);
                    StoreItem(resourceToSpawn);
                    FindObjectOfType<ResourceInventory>().checkForItemAndRemove(resourceToSpawn.GetComponent<rsrce>().nameOfResource);
                }
            }
        }
    }

    public void InitializeInventory()
    {
        for (int s = 0; s < numberOfInventorySlots; s++)
        {
            GameObject slot = Instantiate(IS);
            slot.transform.parent = transform;
            slot.GetComponent<RectTransform>().anchoredPosition = origin + new Vector3(s * spacing, 0, 0);
            slots.Add(slot);
        }
    }

    public void StoreItem(GameObject resource)
    {
        for (int s = 0; s < numberOfInventorySlots; s++)
        {
            if (slots[s].GetComponent<InventorySlot>().item == null)
            {
                slots[s].GetComponent<InventorySlot>().AddItem(resource);
                FindObjectOfType<ResourceInventory>().addItem(resource.GetComponent<rsrce>().nameOfResource);
                if (s == numberOfInventorySlots - 1)
                {
                    isFull = true;
                }
                break;
            }
        }
        
    }

    public void addInventorySlot()
    {
        int offset = 0;
        if (numberOfInventorySlots >= 4)
        {
            offset = 630;
        }
        if (numberOfInventorySlots == 6)
        {
            transform.parent.Find("ShopButton").position += new Vector3(0, 125, 0);
        }
        numberOfInventorySlots++;
        GameObject slot = Instantiate(IS);
        slot.transform.parent = transform;
        slot.GetComponent<RectTransform>().anchoredPosition = origin + new Vector3((numberOfInventorySlots - 1) * spacing + offset, 0, 0);
        slots.Add(slot);
        isFull = false;
    }
}
