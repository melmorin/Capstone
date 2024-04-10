using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    [Header("Dependancies")]
    [SerializeField] private GameObject startPoint; 
    [SerializeField] private TextMeshProUGUI levelText; 
    [SerializeField] private GameObject levelMenu; 
    [SerializeField] private Button playButton; 
    [SerializeField] private GameObject winMenu; 
    [SerializeField] private TextMeshProUGUI coinCountText; 
    [SerializeField] private Image endItem; 
    [SerializeField] private GameObject trophy; 
    [SerializeField] private GameObject lockedMenu; 
    [SerializeField] private TextMeshProUGUI lockedText; 

    [Header("Runtime Vars")]
    public string lastDirX;
    public bool facingLeft = true; 
    public bool hasWarped; 

    private Animator anim; 
    private int currentNodeNumber; 
    private Vector2 movement;
    private SpriteRenderer sprite; 
    private float speed = 3f; 
    private SceneController sceneManager;
    private GameObject player; 

    // Start is called before the first frame update
    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player"); 
        anim = player.GetComponent<Animator>();
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>(); 
        if (sceneManager.lastLevel == "") player.transform.position = startPoint.transform.position; 
        sprite = player.GetComponent<SpriteRenderer>();

        if (facingLeft) sprite.flipX = false; 
        else sprite.flipX = true;
        facingLeft = !facingLeft; 
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
    public void SetMenuActive(bool active, string levelName = "", int node = -1, bool hasWon = false, int maxCoins = 0, int coinCount = 0, Sprite endItemSprite = null)
    {
        levelMenu.SetActive(active); 
        levelText.text = levelName;
        currentNodeNumber = node; 

        winMenu.SetActive(hasWon); 
        coinCountText.text = coinCount + " / " + maxCoins; 
        if (endItemSprite != null) endItem.sprite = endItemSprite; 

        if (maxCoins == coinCount) trophy.SetActive(true);
        else trophy.SetActive(false);
    }

    // Activates the menu for a locked level 
    public void SetLockedMenu(bool activate, int winsNeeded = -1)
    {
        if (activate)
        {
            lockedMenu.SetActive(true);
            if (winsNeeded == 1) lockedText.text = "You need "+ winsNeeded +" more win to access this level";
            else lockedText.text = "You need "+ winsNeeded +" more wins to access this level";
        }
        else lockedMenu.SetActive(false);
    } 

    // Loads the scene to play a level 
    private void PlayGame()
    {
        if (currentNodeNumber == 1)
        {
            GameObject entrancePoint = GameObject.Find("EntrancePoint");
            StartCoroutine(GoToEntrance(entrancePoint)); 
        }
        else StartCoroutine(sceneManager.LoadSceneAnim(currentNodeNumber));
    }

    IEnumerator GoToEntrance(GameObject entrancePoint)
	{
        SetMenuActive(false, "", 1);
		Animate("Down");
		yield return new WaitForSeconds(1/60);
		while (player.transform.position != entrancePoint.transform.position) 
		{
            SetMenuActive(false, "", 1);
			player.transform.position = Vector3.MoveTowards(player.transform.position, entrancePoint.transform.position, speed * Time.deltaTime);
			yield return null;
		}
		Animate("Stop");
        StartCoroutine(sceneManager.LoadSceneAnim(currentNodeNumber));
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
