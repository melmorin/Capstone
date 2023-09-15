using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreen; 
    [SerializeField] private GameObject pauseScreen; 
    public static bool GameIsPaused = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else 
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false; 
    }

    public void Pause()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true; 
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
}
