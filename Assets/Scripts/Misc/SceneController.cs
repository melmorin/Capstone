using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private int currentSceneIndex; 
    private GameManager gameManager;
    private GameObject player;  

    [Header ("Runtime Vars")]
    public bool scooperTalkedTo = false;
    public bool sheriffTalkedTo = false;  

    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>(); 
        player = GameObject.FindGameObjectWithTag("Player"); 
        DontDestroyOnLoad(gameObject);
    }

    // Runs on awake 
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Runs every time a new scene is loaded 
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
            player = GameObject.FindGameObjectWithTag("Player"); 

            if (scene.buildIndex == 1)
            {
                GameObject spawnPoint1 = GameObject.Find("SpawnPoint1");
                GameObject spawnPoint2 = GameObject.Find("SpawnPoint2");

                if (currentSceneIndex == 2)
                {
                    player.transform.position = spawnPoint2.transform.position; 
                    player.GetComponent<TopDownMovement>().movement = new Vector2(0, -1);
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
        }
        
        currentSceneIndex = scene.buildIndex; 
    }
}
