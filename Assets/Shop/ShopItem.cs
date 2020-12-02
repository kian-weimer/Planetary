
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class ShopItem : MonoBehaviour
{
    public string name;
    public int cost;

    public void Generate(ShopItemInfo info)
    {
        name = info.name;
        cost = info.cost;


        Text itemName = transform.Find("Name").GetComponent<Text>();
        itemName.text = info.name;
        Text itemCost = transform.Find("Cost").GetComponent<Text>();
        itemCost.text = info.cost + "";

        Button buy = transform.Find("Buy").GetComponent<Button>();

        buy.onClick.AddListener(delegate () { FindObjectOfType<Shop>().ItemPurchased(this); });
    }
}