using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Dialogue : MonoBehaviour
{
    public static int currentDialogue = 0;
    public static List<string> dialogue;
    [SerializeField] TextAsset file;
    private void Awake()
    {
        dialogue = new List<string>();
        dialogue.AddRange(file.text.Split("\n"[0]));
        for (int i = 0; i < dialogue.Count; i++)
        {
            //Debug.Log(dialogue[i].Substring(0, 1) + " says " + dialogue[i].Substring(3));
        }
    }
}
