using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DraggableWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private Canvas canvas;
    private Color backgroundColor;
    private void Awake()
    {
        if (dragRectTransform == null)
        {
            dragRectTransform = transform.GetComponent<RectTransform>();
        }
        if(canvas == null)
        {
            Transform testCanvas = transform.parent;
            while (testCanvas != null)
            {
                canvas = testCanvas.GetComponent<Canvas>();
                if(canvas != null)
                {
                    break;
                }
                testCanvas = testCanvas.parent;
            }
        }

    }


    public void OnDrag(PointerEventData eventData)
    {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        dragRectTransform.SetAsLastSibling();
    }
}
