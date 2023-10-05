using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

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

    void Awake()
    {
        continueButton.GetComponent<Button>().onClick.AddListener(Nextline);
    }

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

    public void SkipText()
    {
        dialogueText.text = currentDialogue[index];
        StopCoroutine(typingRoutine); 
    }

    public IEnumerator Typing()
    {
        foreach(char letter in currentDialogue[index].ToCharArray())
        {
            dialogueText.text += letter; 
            yield return new WaitForSeconds(wordSpeed); 
        }
    }

    public void ZeroText()
    {
        dialogueText.text = "";
        index = 0; 
        dialoguePanel.SetActive(false);
    }
}
