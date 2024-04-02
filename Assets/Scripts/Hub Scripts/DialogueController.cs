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
    public Sprite[] currentProfiles;
    public GameObject currentNPC;  

    private SceneController sceneController; 

    // Adds listener to the continue button for NextLine()
    void Awake()
    {
        continueButton.GetComponent<Button>().onClick.AddListener(Nextline);
    }

    // Runs on start 
    void Start()
    {
        sceneController = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>(); 
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
        StopAllCoroutines(); 
        continueButton.SetActive(false); 
        skipButton.SetActive(true); 
        if (index < currentDialogue.Length - 1)
        {
            index++; 
            dialogueText.text = ""; 
            StartCoroutine(Typing()); 
        }
        else 
        {
            ZeroText(); 
        }
    }

    // Begins coroutine for the NPC script
    public void StartTypingRoutine()
    {
        StartCoroutine(Typing()); 
    }

    // Stops the typing 
    public void StopTypingRoutine()
    {
        StopAllCoroutines();
    }

    // Stops the typing animation if the player skips the text
    public void SkipText()
    {
        dialogueText.text = currentDialogue[index];
        StopAllCoroutines();
    }

    // The typing animation 
    public IEnumerator Typing()
    {
        profileImage.sprite = currentProfiles[index]; 
        foreach(char letter in currentDialogue[index].ToCharArray())
        {
            dialogueText.text += letter; 
            yield return new WaitForSeconds(wordSpeed); 
        }
    }

    // Closes down entire dialogue menu 
    public void ZeroText()
    {
        StopAllCoroutines();
        dialogueText.text = "";
        if (currentNPC != null)
        {
            NPC npcScript = currentNPC.GetComponent<NPC>(); 
            if (index == currentDialogue.Length - 1) 
            {
                npcScript.readOnce = true; 
                if (npcScript.npcName == "Scooper")
                {
                    sceneController.scooperTalkedTo = true; 
                }
                else if (npcScript.npcName == "The Sheriff")
                {
                    sceneController.sheriffTalkedTo = true; 
                }
            }
            npcScript.gameManager.ToggleButtonPrompt("Press E to Talk"); 
        }
        
        index = 0; 
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);          
        }
    }
}
