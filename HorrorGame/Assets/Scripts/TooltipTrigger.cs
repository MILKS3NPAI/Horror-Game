using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public string content, header;
    private GameObject player;

    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
    }
    private void Update()
    {
        if ((transform.position - player.transform.position).magnitude < 3)
        {
            TooltipSystem.Show(content, header);
        }
        else
        {
            TooltipSystem.Hide();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //TooltipSystem.Show(content, header);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //TooltipSystem.Hide();
    }
}
