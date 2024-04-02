using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class NPC : MonoBehaviour
{
    [Header ("NPC Information")]
    public string npcName; 
    [SerializeField] private Sprite[] npcProfiles; 
    [SerializeField] private Sprite[] npcSecondProfiles; 
    [SerializeField] private string[] dialogue; 
    [SerializeField] private string[] secondDialogue; 

    private bool playerIsClose;
    private DialogueController dialogueController;

    [Header ("Runtime Vars")]
    public bool readOnce = false; 
    public GameManager gameManager;

    // Runs before the first frame 
    void Start()
    {
        dialogueController = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<DialogueController>(); 
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>(); 
    }

    // Runs every frame 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialogueController.dialoguePanel.activeInHierarchy)
            {
                dialogueController.ZeroText(); 
            }
            else 
            {
                dialogueController.dialoguePanel.SetActive(true);  
                gameManager.ToggleButtonPrompt("");
                if (dialogueController.continueButton.activeInHierarchy)
                {
                    dialogueController.continueButton.SetActive(false); 
                }
                if (!dialogueController.skipButton.activeInHierarchy)
                {
                    dialogueController.skipButton.SetActive(true); 
                }
                if (readOnce) 
                {
                    dialogueController.currentDialogue = secondDialogue;
                    dialogueController.currentProfiles = npcSecondProfiles; 
                }
                else 
                {
                    dialogueController.currentDialogue = dialogue; 
                    dialogueController.currentProfiles = npcProfiles;
                }

                dialogueController.currentNPC = gameObject; 
                dialogueController.nameText.text = npcName; 
                dialogueController.StartTypingRoutine();
            }
        }
    }

    // Checks when the player enters the trigger box 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            playerIsClose = true;  
            gameManager.ToggleButtonPrompt("Press E to Talk");
        }
    }

    // Checks when the player leaves the trigger box 
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false; 
            if (dialogueController != null) dialogueController.ZeroText(); 
            gameManager.ToggleButtonPrompt("");
        }
    }
}
