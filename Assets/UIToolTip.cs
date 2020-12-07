using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TreeEntry treeEntry;
    public PopupManager popupManager;
    public void OnPointerEnter(PointerEventData eventData)
    {
        popupManager.movePopup(treeEntry.detailedText, "skill");
        Debug.Log("HERE");
        Debug.Log(treeEntry.detailedText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Player.newCanShoot = false;
        popupManager.movePopup();
    }
}
