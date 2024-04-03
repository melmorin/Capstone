using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header ("If Applicable")]
    [SerializeField] private GameObject deathScreen; 
    [SerializeField] private TextMeshProUGUI coinText; 
    [SerializeField] private TextMeshProUGUI promptText; 
    public Sprite endItem; 
    public string levelName; 

    [Header ("Dependancies")]
    public bool isLevel; 
    [SerializeField] private GameObject pauseScreen; 
    [SerializeField] private GameObject sceneController;

    [Header ("Runtime Vars")]
    public bool gameIsPaused = false; 
    public bool gameOver = false; 
    public int coinCount = 0; 

    // Awake is called before start
    void Awake()
    {
        GameObject sceneObject = GameObject.FindGameObjectWithTag("SceneManager"); 
        if (sceneObject == null)
        {
            Instantiate(sceneController);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }

    public bool IsGamePaused()
    {
        return gameIsPaused;
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false; 
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true; 
    }

    public void AddCoin()
    {
        coinCount += 1; 
        coinText.text = "x " + coinCount; 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void EndGame()
    {
        deathScreen.SetActive(true); 
    }
    public void Death()
    {
        gameOver = true; 
    }

    public void ChangeScene(int screenBuildIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(screenBuildIndex, LoadSceneMode.Single);
    }

    public void ToggleButtonPrompt(string prompt)
    {
        promptText.text = prompt; 
    }
}

