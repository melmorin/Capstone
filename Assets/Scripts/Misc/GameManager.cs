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
    [SerializeField] private GameObject pauseScreen;  
    public Sprite endItem; 
    public string levelName; 

    private float camSpeed = 25f; 
    private Camera mainCamera;

    [Header ("Dependancies")]
    public bool isLevel; 
    [SerializeField] private GameObject sceneControllerObject;

    [Header ("Runtime Vars")]
    public bool gameIsPaused = false; 
    public bool gameOver = false; 
    public int coinCount = 0; 

    private SceneController sceneManager;

    // Awake is called before start
    void Awake()
    {
        GameObject sceneObject = GameObject.FindGameObjectWithTag("SceneManager"); 
        if (sceneObject == null)
        {
            Instantiate(sceneControllerObject);
        }
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>(); 
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); 
    }

    // Moves Camera to new pos
	public IEnumerator MoveCamera(GameObject newCameraPos, bool instantMove)
	{
        if (!instantMove)
        {
            while (mainCamera.transform.position != newCameraPos.transform.position) 
            {
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, newCameraPos.transform.position, camSpeed * Time.deltaTime);
                yield return null;
            }
        }
        else 
        {
            mainCamera.transform.position = newCameraPos.transform.position;
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseScreen != null && !sceneManager.isLoading && !gameOver)
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

    public void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(sceneManager.LoadSceneAnim(currentSceneIndex));
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
        StartCoroutine(sceneManager.LoadSceneAnim(screenBuildIndex));
    }

    public void ToggleButtonPrompt(string prompt)
    {
        promptText.text = prompt; 
    }
}

