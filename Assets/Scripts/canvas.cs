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
    // Start is called before the first frame update
    void Start()
    {
        mainBuyShop.transform.parent.parent.parent.gameObject.SetActive(true);
        mainBuyShop.transform.parent.parent.parent.gameObject.SetActive(false);
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
