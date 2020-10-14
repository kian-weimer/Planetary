using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Canvas canvas;
    RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public bool moved;
    private void Awake()
    {
        canvas = FindObjectOfType<canvas>().GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
        transform.parent.SetAsLastSibling(); // keps icon on top
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;

    }

        public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        if (moved)
        {
            moved = false;
        }
        else
        {
            GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("ONPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
