using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class ReadController : MonoBehaviour
{
    [Header ("Dependancies")]
    public GameObject readPanel; 
    [SerializeField] private TextMeshProUGUI readTextBox; 
    [SerializeField] private float wordSpeed;

    private GameManager gameManager; 

    [Header ("Runtime Vars")]
    public Coroutine typingRoutine; 

    // Plays on start 
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>(); 
    }

    // Starts coroutine 
    public void StartCurrentRoutine(string text)
    {
        typingRoutine = StartCoroutine(Typing(text));
    }

    // The typing animation 
    public IEnumerator Typing(string signText)
    {
        foreach(char letter in signText.ToCharArray())
        {
            readTextBox.text += letter; 
            yield return new WaitForSeconds(wordSpeed); 
        }
    }

     // Closes down entire sign menu 
    public void ZeroText()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        readTextBox.text = "";
        if (readPanel != null)
        {
            readPanel.SetActive(false);          
        }
        gameManager.ToggleButtonPrompt("Press E");
    }
}
