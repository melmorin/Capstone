using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen; 
    [SerializeField] private GameObject pauseScreen; 
    [SerializeField] private TextMeshProUGUI coinText; 
    public static bool gameIsPaused = false; 
    public bool gameOver = false; 
    private int coinCount = 0; 

    // Start is called before the first frame update
    void Start()
    {
        
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
        SceneManager.LoadScene(screenBuildIndex, LoadSceneMode.Single);
    }
}
