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

    [Header("Runtime Vars")]
    private Animator anim; 
    public bool facingLeft = true; 
    private int currentNodeNumber; 
    private Vector2 movement;
    private Vector2 lastDirection; 
    private SpriteRenderer sprite; 

    // Start is called before the first frame update
    void Start()
    { 
        anim = player.GetComponent<Animator>();
        player.transform.position = startPoint.transform.position; 
        sprite = player.GetComponent<SpriteRenderer>();
    }

    // Called when activated 
    void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
    }

    // Flips the player depending on the movement direction
    public void Flip()
    {
        if (facingLeft) sprite.flipX = false; 
        else sprite.flipX = true;
        facingLeft = !facingLeft; 
    }

    // Displays the menu for a level 
    public void SetMenuActive(bool active, string levelName = "", int node = -1)
    {
        levelMenu.SetActive(active); 
        levelText.text = levelName;
        currentNodeNumber = node; 
    }

    // Loads the scene to play a level 
    private void PlayGame()
    {
        SceneManager.LoadScene(currentNodeNumber, LoadSceneMode.Single);
    }

    // Changes player animations based on the direction given
    public void Animate(string move)
    {
        if (move == "Stop")
        {
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Side", false);
            return; 
        }

        else if (move == "Right" || move == "Left")
        {
            anim.SetBool("Side", true);
            return; 
        }

        anim.SetBool(move, true);
    }
}
