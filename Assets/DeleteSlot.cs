using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DeleteSlot : MonoBehaviour, IDropHandler
{
    public ResourceInventory RI;
    public GameObject circleTimer;
    public UnityEngine.UI.Image timer;
    public float waitTime;
    public bool deleting = false;
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
        if (! gameObject.activeSelf) { return; }
        circleTimer.gameObject.SetActive(false);
        gameObject.SetActive(false);

        InventorySlot delInv = transform.GetComponent<InventorySlot>();
        if (delInv.item != null)
        {
            // reduce inventory here
            RI.resourceList[delInv.item.GetComponent<rsrce>().nameOfResource] -= 1;

            if (delInv.icon.GetComponent<DragDrop>() != null)
            {
                delInv.icon.GetComponent<DragDrop>().destroy();
            }
            else
            {
                delInv.icon.GetComponent<DragDropSpawnable>().destroy();
            }
            Destroy(delInv.item);
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
}
