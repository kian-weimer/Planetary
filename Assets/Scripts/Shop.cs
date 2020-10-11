using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Font font;
    public List<ShopItemInfo> items; // first is name second is cost third is function call
    public int itemDistance = 1;
    public GameObject text;
    public GameObject shopItem;
    RectTransform rT;
    // Start is called before the first frame update
    void Start()
    {
        rT = GetComponent<RectTransform>();
        // rT.sizeDelta = new Vector2(rT.sizeDelta.x, items.Count * itemDistance);
        for (int i = 0; i < items.Count; i++)
        {
           
            GameObject item = Instantiate(shopItem);
            item.name = "Item " + i;
            item.transform.SetParent(transform);
            item.transform.position -= new Vector3(-1545, -500 + i * itemDistance , 0); // not sure why negative numbers there are needed...

            if (items[i].sellItem)
            {
                item.GetComponent<SellShopItem>().Generate(items[i]);
            }
            else
            {
                item.GetComponent<ShopItem>().Generate(items[i]);
            }
            /*
            item.GetComponent<ShopItem>().name = items[i].name; 
            item.GetComponent<ShopItem>().cost = items[i].cost;


            Text itemName = item.transform.Find("Name").GetComponent<Text>();
            itemName.text = items[i].name;
            Text itemCost = item.transform.Find("Cost").GetComponent<Text>();
            itemCost.text = items[i].cost + "";

            Button buy= item.transform.Find("Buy").GetComponent<Button>();

            buy.onClick.AddListener(delegate () { ItemPurchased(item.GetComponent<ShopItem>()); });

            
            GameObject textEntry = Instantiate(text);
            textEntry.name = "Item " + i;
            textEntry.transform.SetParent(transform);
            textEntry.transform.position -= new Vector3(-1545, -400 + i * itemDistance, 0); // not sure why negative numbers there are needed...
            //textEntry.RectT = new Vector2(1, 0);
            //textEntry.transform.anchorMax = new Vector2(0, 1);
            //textEntry.transform.pivot = new Vector2(0.5f, 0.5f);


            Text myText = textEntry.GetComponent<Text>();
            myText.text = items[i];
            */
        }

        // FIX THIS _------------------
        //FindObjectOfType<Scrollbar>().gameObject.GetComponent<Scrollbar>().SetValueWithoutNotify(1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ItemPurchased(ShopItem item)
    {
        Debug.Log(item.name + " " +  item.cost);
    }

    public void ItemSold(SellShopItem item)
    {
        Debug.Log(item.name + " " + item.cost + " " + item.quantity);
    }
}
