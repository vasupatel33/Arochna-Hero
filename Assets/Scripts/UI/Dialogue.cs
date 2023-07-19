using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [Header("Starting Dialogue")]
        [SerializeField] private string[] startingDialogue;

    [Header("Attributes")]
        [SerializeField] private float waitTime;
        [SerializeField] private string endNotice;
        private bool running;
        private bool skip;

    [Header("References")]
        private Animator dialogueAnimation;
        private TextMeshProUGUI dialogueText;

    void Awake()
    {
        dialogueAnimation = GameObject.Find("Canvas").transform.Find("ChatBoxContainer").Find("ChatPanel").GetComponent<Animator>();
        dialogueText = GameObject.Find("Canvas").transform.Find("ChatBoxContainer").Find("ChatPanel").Find("DialogueText").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        if (startingDialogue.Length == 0) { return; }

        OpenDialogue(startingDialogue);
    }

    public void OpenDialogue(string[] text)
    {
        StopAllCoroutines();
        dialogueAnimation.Play("Open");
        StartCoroutine(WriteText(text));
    }

    private void CloseDialogue()
    {
        StopAllCoroutines();
        running = false;
        dialogueAnimation.Play("Close");
    }

    private IEnumerator WriteText(string[] text, string objective = "")
    {
        running = true;

        foreach (string dialogue in text)
        {
            dialogueText.text = "";

            skip = false;

            foreach (char letter in dialogue)
            {
                yield return new WaitForSeconds(waitTime);
                dialogueText.text += letter;

                if (skip)
                {
                    dialogueText.text = dialogue;
                    skip = false;
                    break;
                }
            }

            dialogueText.text += " ";

            foreach (char letter in endNotice)
            {
                yield return new WaitForSeconds(waitTime);
                dialogueText.text += letter;
            }
            
            while (!Input.GetKeyDown(KeyCode.Return))
            {
                yield return null;
            }
        }

        CloseDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && running)
            skip = true;
    }
}
