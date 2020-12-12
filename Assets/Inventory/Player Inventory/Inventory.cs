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

    public List<EnemyController> friendliesForSave;
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

        //for storing the friendly
        EnemyController[] friendlies = FindObjectOfType<Home>().GetComponentsInChildren<EnemyController>();
        List<string> friendiesSaveStrings = new List<string>();
        foreach (EnemyController friendly in friendlies)
        {

            if(friendly.gameObject.name == "Friend(Clone)")
            {
                friendiesSaveStrings.Add(friendly.gameObject.transform.position.x + "," + friendly.gameObject.transform.position.y + ",Friend");
            }
            else if (friendly.gameObject.name == "FriendDecoy(Clone)")
            {
                friendiesSaveStrings.Add(friendly.gameObject.transform.position.x + "," + friendly.gameObject.transform.position.y + ",FriendDecoy");
            }
            else if (friendly.gameObject.name == "FriendTurrret(Clone)")
            {
                friendiesSaveStrings.Add(friendly.gameObject.transform.position.x + "," + friendly.gameObject.transform.position.y + ",FriendTurret");
            }
        }

        //Debug.Log(friendiesSaveStrings[0]);

        FileStream fs2 = new FileStream("savedFriendlies.dat", FileMode.Create);
        BinaryFormatter bf2 = new BinaryFormatter();
        bf2.Serialize(fs2, friendiesSaveStrings);
        fs2.Close();
    }

    public void Load()
    {
        List<string> names = new List<string>();
        using (Stream stream = File.Open("savedInventory.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            names = (List<string>)bformatter.Deserialize(stream);
        }

        InventorySlot[] inventorySlots = GetComponentsInChildren<InventorySlot>();
        foreach (InventorySlot slot in inventorySlots)
        {
            if (slot.gameObject.name == "InventorySlot(Clone)")
            {
                GameObject item = slot.gameObject;
                Destroy(item);
            }
        }        
        slots.Clear();
        int numberOfInventorySlotsToAdd = PlayerPrefs.GetInt("NumberOfInventorySlots");

        InitializeInventory();
        for (int i  = 0; i < numberOfInventorySlotsToAdd - 2; i++)
        {
            addInventorySlot();
        }

        foreach(string name in names)
        {
            foreach(GameObject resource in resources)
            {
                if (name == resource.name)
                {
                    GameObject resourceToSpawn = Instantiate(resource);
                    StoreItem(resourceToSpawn);
                    FindObjectOfType<ResourceInventory>().checkForItemAndRemove(resourceToSpawn.GetComponent<rsrce>().nameOfResource);
                }
            }
        }

        List<string> freindlies = new List<string>();
        using (Stream stream = File.Open("savedFriendlies.dat", FileMode.Open))
        {
            var bformatter = new BinaryFormatter();
            freindlies = (List<string>)bformatter.Deserialize(stream);
        }

        foreach(string friendly in freindlies)
        {

            string[] components = friendly.Split(',');

            if(components[2] == "Friend")
            {
                GameObject friend = Instantiate(friendliesForSave[0].gameObject);

                Vector3 positionToSpawn = new Vector3();
                positionToSpawn.x = float.Parse(components[0]);
                positionToSpawn.y = float.Parse(components[1]);
                positionToSpawn.z = 0;

                friend.transform.position = positionToSpawn;
            }
            else if (components[2] == "FriendDecoy")
            {
                GameObject friend = Instantiate(friendliesForSave[1].gameObject);

                Vector3 positionToSpawn = new Vector3();
                positionToSpawn.x = float.Parse(components[0]);
                positionToSpawn.y = float.Parse(components[1]);
                positionToSpawn.z = 0;

                friend.transform.position = positionToSpawn;
            }
            else if (components[2] == "FriendTurret")
            {
                GameObject friend = Instantiate(friendliesForSave[2].gameObject);

                Vector3 positionToSpawn = new Vector3();
                positionToSpawn.x = float.Parse(components[0]);
                positionToSpawn.y = float.Parse(components[1]);
                positionToSpawn.z = 0;

                friend.transform.position = positionToSpawn;
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
