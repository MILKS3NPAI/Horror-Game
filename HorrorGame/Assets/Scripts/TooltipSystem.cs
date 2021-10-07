using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    [SerializeField] Tooltip tooltip;
    private static TooltipSystem current;

    public void Awake()
    {
        current = this;
    }

    public static void Show(string content, string header = "")
    {
        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
    }
    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
