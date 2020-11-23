using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenEvent : MonoBehaviour
{
    public GameObject player;
    public bool eventOcurring = false;
    public BroadcastMessage BM;
    public float decrementRate;
    public float startingFillAmount;
    public OxygenSlot oxygenSlot;
    IEnumerator waitSeconds(float seconds)
    {
        yield return seconds;
    }

    public void startEvent(OxygenSlot oxygenSlot)
    {
        this.oxygenSlot = oxygenSlot;
        transform.localScale = new Vector3(transform.localScale.x * startingFillAmount, transform.localScale.y, transform.localScale.z);
        transform.parent.parent.gameObject.SetActive(true);
        eventOcurring = true;
        BM.Broadcast("Oxygen stores are running low!");
        waitSeconds(3);
        BM.Broadcast("Place oxgen in tank to replenish stores.");
    }

    private void Update()
    {
        if (eventOcurring)
        {
            if (transform.localScale.x >= 1)
            {
                player.GetComponent<Player>().addExpPoints(50);
                endEvent();
                BM.Broadcast("Oxygen stores have been replenished!");
            }
            transform.localScale = new Vector3(transform.localScale.x - decrementRate, transform.localScale.y, transform.localScale.z);
            if (transform.localScale.x <= 0)
            {
                player.GetComponent<Player>().loseHealth(10000);
                endEvent();
                BM.Broadcast("Oxygen stores were depleated!");
            }
        }
    }

    public void endEvent()
    {
        eventOcurring = false;
        oxygenSlot.eventOcurring = false;
        transform.parent.parent.gameObject.SetActive(false);
    }
}
