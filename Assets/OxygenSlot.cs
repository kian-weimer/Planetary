using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OxygenSlot : MonoBehaviour, IDropHandler
{
    public ResourceInventory RI;
    public GameObject circleTimer;
    public UnityEngine.UI.Image timer;

    public bool eventOcurring = false;

    public float waitTime;
    public bool deleting = false;
    public GameObject player;
    public OxygenEvent oxygenEvent;

    public GameObject oxygenBar;
    public float addedOxygenIncrementAmount;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            deleting = true;
            InventorySlot delInv = transform.GetComponent<InventorySlot>();
            circleTimer.gameObject.SetActive(true);
            circleTimer.transform.SetAsLastSibling();
            StartCoroutine(ExampleCoroutine(waitTime));
        }
    }

    public void delete()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(wait());
        }
    }

    public void deleteNow()
    {
        circleTimer.gameObject.SetActive(false);
        gameObject.SetActive(false);

        InventorySlot delInv = transform.GetComponent<InventorySlot>();
        if (delInv.item != null)
        {
            // reduce inventory here
            RI.resourceList[delInv.item.GetComponent<rsrce>().nameOfResource] -= 1;

            delInv.icon.GetComponent<DragDrop>().destroy();
            Destroy(delInv.item);
            if (oxygenBar.transform.localScale.x + addedOxygenIncrementAmount > 1)
            {
                oxygenBar.transform.localScale = new Vector3(1, oxygenBar.transform.localScale.y, oxygenBar.transform.localScale.z);

            }
            else
            {
                oxygenBar.transform.localScale = new Vector3(oxygenBar.transform.localScale.x + addedOxygenIncrementAmount, oxygenBar.transform.localScale.y, oxygenBar.transform.localScale.z);

            }
            delInv.RemoveItem();
            FindObjectOfType<Player>().regenerateFromDeletedItem();
        }
        deleting = false;
    }

    IEnumerator ExampleCoroutine(float duration)
    {
        float start = Time.time;
        float time = duration;
        float value = 1;
        //yield on a new YieldInstruction that waits for 5 seconds.
        while (Time.time - start < waitTime)
        {
            time -= Time.deltaTime;
            value = time / duration;
            timer.fillAmount = value;
            yield return null;
        }
        deleteNow();
    }

    IEnumerator wait()
    {
        yield return .1f;
        if (!deleting)
        {
            gameObject.SetActive(false);
        }
    }

    public void startEvent() // loops like this because inv slot is easier to search for
    {
        eventOcurring = true;
        oxygenEvent.startEvent(this);
    }
}
