using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Option : MonoBehaviour
{
    private GameManager gameManager;
    private bool playerIsClose = false; 
    [SerializeField] private GameObject leavePanel; 


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            TogglePanel(); 
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
            gameManager.ToggleButtonPrompt("");
            leavePanel.SetActive(false);
        }
    }

    // Hides panel 
    public void TogglePanel()
    {
        if (leavePanel.activeInHierarchy)
        {
            leavePanel.SetActive(false);
            if (playerIsClose) gameManager.ToggleButtonPrompt("Press E");
        }
        else 
        {
            leavePanel.SetActive(true);
            gameManager.ToggleButtonPrompt("");
        }
    }
}
