using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI buttonText;

    void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.fontSize = 90;
        buttonText.faceColor = new Color32(0, 0, 0, 255);
        buttonText.outlineWidth = 0.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.fontSize = 75;
        buttonText.faceColor = new Color32(255, 255, 255, 255);
        buttonText.outlineWidth = 0;
    }
}
