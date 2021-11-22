using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public static GameObject tooltipObject;
    public string content, header;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //TooltipSystem.Show(content, header);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //TooltipSystem.Hide();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!content.Contains("[") && !content.Contains("]"))
                content = "[" + InputManager.interactKey + "] " + content;
            tooltipObject = gameObject;
            TooltipSystem.Show(content, header);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TooltipSystem.Hide();
        }
    }
}
