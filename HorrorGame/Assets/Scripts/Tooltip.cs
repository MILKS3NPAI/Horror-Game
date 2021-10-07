using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public LayoutElement layoutElement;
    public Text contentField;
    private int charWrapLimit = 20;

    private void Update()
    {
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (contentLength > charWrapLimit) ? true : false;
    }
}
