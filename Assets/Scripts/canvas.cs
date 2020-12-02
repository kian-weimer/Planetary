using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvas : MonoBehaviour
{
    public BroadcastMessage broadcaster;
    public GameObject friendlySellShop;
    public GameObject friendlyBuyShop;
    public GameObject friendlyShopButton;
    public GameObject FriendlyShop;
    public Shop mainBuyShop;
    public Shop mainSellShop;
    public GameObject WholeShop;
    // Start is called before the first frame update
    void Start()
    {
        mainBuyShop.loadUp();
        mainSellShop.loadUp();
        //StartCoroutine(waitTime());
    }

    public void broadcast(string message)
    {
        broadcaster.Broadcast(message);
    }

    public void enableFriendlyTraderShopButton()
    {
        friendlyShopButton.SetActive(true);
    }

    public void disableFriendlyTraderShopButton()
    {
        friendlyShopButton.SetActive(false);
    }

    public void closeTrader()
    {
        friendlyShopButton.SetActive(false);
        friendlySellShop.SetActive(false);
        friendlyBuyShop.SetActive(true);
        FriendlyShop.SetActive(false);
    }
}
