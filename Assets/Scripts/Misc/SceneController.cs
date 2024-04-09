using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private int currentSceneIndex; 
    private GameManager gameManager;
    private GameObject player;  

    [Header ("Dependancies")]
    [SerializeField] private Animator levelLoaderAnim; 
    [SerializeField] private float transitionTime = 1f; 

    [Header ("Runtime Vars")]
    public bool scooperTalkedTo = false;
    public bool sheriffTalkedTo = false;  
    public string lastLevel = ""; 
    public List<LevelData> levelInfo = new List<LevelData>(); 
    public bool isLoading; 

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>(); 
        player = GameObject.FindGameObjectWithTag("Player"); 
        DontDestroyOnLoad(gameObject);

        if (gameManager.isLevel) WhenLevel(); 
    }

    // loads a scene with its animation
    public IEnumerator LoadSceneAnim(int index)
    {
        isLoading = true; 
        levelLoaderAnim.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    // Makes the loading bool false; 
    public IEnumerator EndLoading()
    {
        levelLoaderAnim.SetTrigger("End");
        yield return new WaitForSeconds(transitionTime);
        isLoading = false; 
    }

    // Runs on awake 
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    // Runs every time a new scene is loaded 
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(EndLoading());

        if (scene.buildIndex != 0)
        {
            gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();

            player = GameObject.FindGameObjectWithTag("Player"); 

            if (scene.buildIndex == 1)
            {
                lastLevel = ""; 
                GameObject spawnPoint1 = GameObject.Find("SpawnPoint1");
                GameObject spawnPoint2 = GameObject.Find("SpawnPoint2");

                if (currentSceneIndex == 2)
                {
                    player.transform.position = spawnPoint2.transform.position; 
                    player.GetComponent<TopDownMovement>().movement = new Vector2(0, -1);
                    player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1.5f); 
                }
                else 
                {
                    player.transform.position = spawnPoint1.transform.position;
                }

                NPC[] npcs = FindObjectsOfType(typeof(NPC)) as NPC[];

                for (int i = 0; i < npcs.Length; i++)
                { 
                    if (npcs[i].npcName == "Scooper")
                    {
                        npcs[i].readOnce = scooperTalkedTo; 
                    }
                    else if (npcs[i].npcName == "The Sheriff")
                    {
                        npcs[i].readOnce = sheriffTalkedTo;  
                    }
                }
            }
            
            else if (gameManager.isLevel) WhenLevel(); 
            
            else if (scene.buildIndex == 2)
            {
                Node[] nodes = FindObjectsOfType(typeof(Node)) as Node[];
                int index = -1; 
                for (int i = 0; i < nodes.Length; i++)
                { 
                    if (nodes[i].levelName == lastLevel){
                        index = i; 
                    } 
                }

                if (index != -1) 
                {
                    player.transform.position = nodes[index].gameObject.transform.position;
                } 
            }

            else 
            {
                lastLevel = ""; 
            }

        }
        
        currentSceneIndex = scene.buildIndex; 
    }

    // Runs when the scene is a level
    void WhenLevel()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin"); 

        int count = 0;
        for(int i = 0; i < levelInfo.Count; i++)
        {   
            if (levelInfo[i].levelName == gameManager.levelName) count++; 
        }

        if (count == 0) levelInfo.Add(new LevelData(gameManager.levelName, false, coins.Length, 0, gameManager.endItem)); 

        lastLevel = gameManager.levelName; 
    }

    // Runs when the player beats a level
    public void LevelWin()
    {
        int index = -1;
        for(int i = 0; i < levelInfo.Count; i++)
        {   
            if (levelInfo[i].levelName == gameManager.levelName) index = i; 
        }

        levelInfo[index].hasWon = true; 
        if (gameManager.coinCount > levelInfo[index].coinsFound) levelInfo[index].coinsFound = gameManager.coinCount;    
    }

    // returns how many levels the player has won
    public int LevelsWon()
    {
        int index = 0;
        for(int i = 0; i < levelInfo.Count; i++)
        {   
            if (levelInfo[i].hasWon) index ++; 
        }
        return index; 
    }

}

// The class for the array that holds level completion data
public class LevelData
{
    public string levelName; 
    public bool hasWon;
    public int maxCoins; 
    public int coinsFound; 
    public Sprite endItem; 

    // contstructor for LevelData 
    public LevelData(string ln, bool hw, int mc, int cf, Sprite ei)
    {
        levelName = ln; 
        hasWon = hw;
        maxCoins = mc; 
        coinsFound = cf; 
        endItem = ei;
    }
}

