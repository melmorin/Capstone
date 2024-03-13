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
    [SerializeField] private Sprite npcSprite; 
    [SerializeField] private string[] dialogue; 
    [SerializeField] private string[] secondDialogue; 

    private SpriteRenderer sprite; 
    private bool playerIsClose;
    private DialogueController dialogueController;
    private GameManager gameManager;

    [Header ("Runtime Vars")]
    public bool readOnce = false; 

    // Runs before the first frame 
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>(); 
        sprite.sprite = npcSprite; 
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
                if (readOnce) dialogueController.currentDialogue = secondDialogue; 
                else dialogueController.currentDialogue = dialogue; 
                dialogueController.profileImage.sprite = npcProfile; 
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
            gameManager.ToggleButtonPrompt("Press E");
        }
    }

    // Checks when the player leaves the trigger box 
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false; 
            dialogueController.ZeroText(); 
            gameManager.ToggleButtonPrompt("");
        }
    }
}
