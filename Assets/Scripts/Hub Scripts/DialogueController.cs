using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 
using System; 

public class DialogueController : MonoBehaviour
{

    [Header ("Dependancies")]
    public GameObject dialoguePanel; 
    public TextMeshProUGUI dialogueText; 
    public TextMeshProUGUI nameText;
    public GameObject continueButton; 
    public Image profileImage;
    public GameObject skipButton; 

    [Header ("Settings")]
    [SerializeField] private float wordSpeed;

    [Header ("Runtime Public Vars")]
    public int index = 0;  
    public string[] currentDialogue;
    public Coroutine typingRoutine; 
    public GameObject currentNPC;  

    // Adds listener to the continue button for NextLine()
    void Awake()
    {
        continueButton.GetComponent<Button>().onClick.AddListener(Nextline);
    }

    // Updates every frame 
    void Update()
    {
        if (currentDialogue.Length - 1 >= index)
        {
            if (dialogueText.text == currentDialogue[index])
            {
                continueButton.SetActive(true); 
                skipButton.SetActive(false); 
            }
        }
    }

    // Moves onto the next line of dialogue or closes window 
    private void Nextline()
    {
        continueButton.SetActive(false); 
        skipButton.SetActive(true); 
        if (index < currentDialogue.Length - 1)
        {
            index++; 
            dialogueText.text = ""; 
            typingRoutine = StartCoroutine(Typing()); 
        }
        else 
        {
            ZeroText(); 
        }
    }

    // Begins coroutine for the BPC script
    public void StartTypingRoutine()
    {
        typingRoutine = StartCoroutine(Typing()); 
    }

    // checks if equal
    public bool IsEqual()
    {
        if (index == currentDialogue.Length - 1) return true; 
        else return false;  
    }

    // Stops the typing animation if the player skips the text
    public void SkipText()
    {
        dialogueText.text = currentDialogue[index];
        StopCoroutine(typingRoutine); 
    }

    // The typing animation 
    public IEnumerator Typing()
    {
        foreach(char letter in currentDialogue[index].ToCharArray())
        {
            dialogueText.text += letter; 
            yield return new WaitForSeconds(wordSpeed); 
        }
    }

    // Closes down entire dialogue menu 
    public void ZeroText()
    {
        StopCoroutine(typingRoutine); 
        dialogueText.text = "";
        if (index == currentDialogue.Length - 1) currentNPC.GetComponent<NPC>().readOnce = true; 
        index = 0; 
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);          
        }
    }
}
