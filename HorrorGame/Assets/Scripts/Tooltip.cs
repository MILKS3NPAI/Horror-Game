using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public LayoutElement layoutElement;
    public RectTransform rectTransform;
    public Text headerField, contentField;
    private GameObject player;
    private int charWrapLimit = 20;
    
    private void Awake()
    {
        player = FindObjectOfType<Player>().gameObject;
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > charWrapLimit || contentLength > charWrapLimit) ? true : false;
        }
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Camera.main.farClipPlane * .5f;
        //Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePos);

        float pivotX = mousePos.x / Screen.width;
        float pivotY = mousePos.y / Screen.height;

        //rectTransform.pivot = new Vector2(pivotX, pivotY);
        //transform.position = mousePos;
        transform.position = player.transform.position + new Vector3(0, 2, 0);
    }
    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        contentField.text = content;
    }
}
