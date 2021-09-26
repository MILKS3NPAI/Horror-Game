using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public static int currentDialogue = 0;
    public static string[] dialogueSpeakers;
    private void Awake()
    {
        dialogueSpeakers = new string[transform.childCount];
        int i = 0;
        foreach (Transform t in transform.GetComponent<Transform>())
        {
            if (transform.GetChild(i).gameObject.name.Contains("Player"))
            {
                dialogueSpeakers[i] = "p";
            }
            else
            {
                dialogueSpeakers[i] = "e";
            }
            i++;
        }
    }
}
