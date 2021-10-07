using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public static List<string> dialogueSequence;
    public static int currentDialogue = 0;
    [SerializeField] LayoutElement layoutElement;
    [SerializeField] Text headerField, contentField;
    [SerializeField] TextAsset file;
    private GameObject dialogueBox, enemy, player;
    private int charWrapLimit = 20;
    private void Awake()
    {
        dialogueBox = transform.GetChild(0).gameObject;
        enemy = FindObjectOfType<Enemy>().gameObject;
        player = FindObjectOfType<Player>().gameObject;

        dialogueSequence = new List<string>();
        dialogueSequence.AddRange(file.text.Split("\n"[0]));
        for (int i = 0; i < dialogueSequence.Count; i++)
        {
            //Debug.Log(dialogue[i].Substring(0, 1) + " says " + dialogue[i].Substring(3));
        }
    }
    private void Update()
    {
        if (Application.isEditor)
        {
            int headerLength = headerField.text.Length;
            int contentLength = contentField.text.Length;

            layoutElement.enabled = (headerLength > charWrapLimit || contentLength > charWrapLimit) ? true : false;
        }
    }
    public void ShowDialogue()
    {
        dialogueBox.SetActive(true);
        if (dialogueSequence[currentDialogue].Substring(0, 1).Equals("P"))
        {
            transform.position = player.transform.position + new Vector3(0, 2, 0);
            dialogueBox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Player";
        }
        else if (dialogueSequence[Dialogue.currentDialogue].Substring(0, 1).Equals("E"))
        {
            transform.position = enemy.transform.position + new Vector3(0, 2, 0);
            dialogueBox.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Enemy";
        }
        dialogueBox.transform.GetChild(1).GetComponent<Text>().text =
            dialogueSequence[currentDialogue].Substring(3);
        currentDialogue++;
    }
    public void HideDialogue()
    {
        dialogueBox.SetActive(false);
        currentDialogue = 0;
    }
}
