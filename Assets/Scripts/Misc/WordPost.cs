using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordPost : MonoBehaviour
{
    [Header ("Sign Information")]
    [SerializeField] private string readText; 
    private bool playerIsClose = false;
    private ReadController readController;  
    private GameManager gameManager;
    private SceneController sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        readController = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<ReadController>(); 
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>(); 
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsClose && !sceneManager.isLoading && !gameManager.gameIsPaused)
        {
            if (readController.readPanel.activeInHierarchy)
            {
                readController.ZeroText(); 
            }
            else 
            {
                readController.readPanel.SetActive(true);  
                gameManager.ToggleButtonPrompt("");
                readController.StartCurrentRoutine(readText);
            }
        }
    }

    // Checks when the player enters the trigger box 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !sceneManager.isLoading && !gameManager.gameIsPaused) 
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
            readController.ZeroText(); 
            gameManager.ToggleButtonPrompt("");
        }
    }
}
