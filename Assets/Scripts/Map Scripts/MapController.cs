using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    [Header("Dependancies")]
    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject startPoint; 
    [SerializeField] private TextMeshProUGUI levelText; 
    [SerializeField] private GameObject levelMenu; 
    [SerializeField] private Button playButton; 

    public bool facingRight = true; 
    private int currentNodeNumber; 

    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = startPoint.transform.position; 
    }

    void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
    }

    // Flips the player depending on the movement direction
    public void Flip()
    {
        facingRight = !facingRight; 
        player.transform.Rotate(0f, 180f, 0f);
    }

    public void SetMenuActive(bool active, string levelName = "", int node = -1)
    {
        levelMenu.SetActive(active); 
        levelText.text = levelName;
        currentNodeNumber = node; 
    }

    void PlayGame()
    {
        SceneManager.LoadScene(currentNodeNumber, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
