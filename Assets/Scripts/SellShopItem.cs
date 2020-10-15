using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellShopItem : MonoBehaviour
{
    public string name;
    public int cost;
    public int quantity;
    public bool sellItem;

    public void QuantityChanged(InputField valueText)
    {
        quantity = int.Parse(valueText.text);
    }

    public void Generate(ShopItemInfo info)
    {
        name = info.name;
        cost = info.cost;
        quantity = 1;


        Text itemName = transform.Find("Name").GetComponent<Text>();
        itemName.text = info.name;
        Text itemCost = transform.Find("Value").GetComponent<Text>();
        itemCost.text = info.cost + "";

        Button buy = transform.Find("Sell").GetComponent<Button>();

        buy.onClick.AddListener(delegate () { FindObjectOfType<Shop>().ItemSold(this); });
    }
}
