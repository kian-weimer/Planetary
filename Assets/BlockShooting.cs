using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockShooting : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static bool forceBlocking = false; // used to force blocking even when over this panel
    public void OnPointerEnter(PointerEventData eventData)
    {
        Player.newCanShoot = true && !forceBlocking;
        Debug.Log("Blocked!");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Player.newCanShoot = false;
    }
}
