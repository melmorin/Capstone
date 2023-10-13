using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class NPC : MonoBehaviour
{
    [Header ("NPC Information")]
    [SerializeField] private string npcName; 
    [SerializeField] private Sprite npcProfile; 
    [SerializeField] private string[] dialogue; 

    private bool playerIsClose;
    private DialogueController dialogueController;

    // Runs before the first frame 
    void Start()
    {
        dialogueController = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<DialogueController>(); 
    }

    // Runs every frame 
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (dialogueController.dialoguePanel.activeInHierarchy)
            {
                StopCoroutine(dialogueController.typingRoutine);
                dialogueController.ZeroText(); 
            }
            else 
            {
                dialogueController.dialoguePanel.SetActive(true); 
                dialogueController.currentDialogue = dialogue; 
                dialogueController.profileImage.sprite = npcProfile; 
                dialogueController.nameText.text = npcName; 
                dialogueController.typingRoutine = StartCoroutine(dialogueController.Typing()); 
            }
        }

        if (dialogue.Length - 1 >= dialogueController.index)
        {
            if (dialogueController.dialogueText.text == dialogue[dialogueController.index])
            {
                dialogueController.continueButton.SetActive(true); 
                dialogueController.skipButton.SetActive(false); 
            }
        }
    }

    // Checks when the player enters the trigger box 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            playerIsClose = true;  
        }
    }

    // Checks when the player leaves the trigger box 
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false; 
            dialogueController.ZeroText(); 
        }
    }
}
