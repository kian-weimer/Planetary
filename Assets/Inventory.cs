using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> slots;
    public int numberOfInventorySlots = 1;

    public Vector3 origin = new Vector3(80, 80, 0);
    public int spacing = 80;

    public bool isFull = false;

    public GameObject IS;
    // Start is called before the first frame update
    void Start()
    {
        InitializeInventory();
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

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
