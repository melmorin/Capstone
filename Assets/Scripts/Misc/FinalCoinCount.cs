using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCoinCount : MonoBehaviour
{
    [Header ("No Coins")]
    [SerializeField] private Sprite[] noCoinsNpcProfiles; 
    [SerializeField] private Sprite[] noCoinsNpcSecondProfiles; 
    [SerializeField] private string[] noCoinsDialogue; 
    [SerializeField] private string[] noCoinsSecondDialogue; 

    [Header ("Yes Coins")]
    [SerializeField] private Sprite[] coinsNpcProfiles; 
    [SerializeField] private Sprite[] coinsNpcSecondProfiles; 
    [SerializeField] private string[] coinsDialogue; 
    [SerializeField] private string[] coinsSecondDialogue; 

    private SceneController sceneManager;
    private GameManager gameManager;
    private NPC npc; 

    private bool coinTalk = false; 

    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>(); 
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
        npc = gameObject.GetComponent<NPC>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (HasAllCoins())
        {
            npc.npcProfiles = coinsNpcProfiles;
            npc.dialogue = coinsDialogue;
            npc.secondDialogue = coinsSecondDialogue; 
            npc.npcSecondProfiles = coinsNpcSecondProfiles; 
        }
        else 
        {
            npc.npcProfiles = noCoinsNpcProfiles;
            npc.dialogue = noCoinsDialogue;
            npc.secondDialogue = noCoinsSecondDialogue; 
            npc.npcSecondProfiles = noCoinsNpcSecondProfiles; 
        }

        if (npc.readOnce && !coinTalk)
        {
            ReadText(); 
        }
    }

    // Check if read coin text 
    private void ReadText()
    {
        if (HasAllCoins() && npc.dialogue == coinsDialogue)
        {
            sceneManager.hasUltimatePrize = true; 
        }
        coinTalk = true; 
    }

    // Checks if the player has gotten all coins. 
    private bool HasAllCoins()
    {
        int totalMaxCoins = 0;
        int totalCurrentCoins = 0;
        for(int i = 0; i < sceneManager.levelInfo.Count; i++)
        {   
            totalMaxCoins += sceneManager.levelInfo[i].maxCoins; 
            totalCurrentCoins += sceneManager.levelInfo[i].coinsFound; 
        }

        if (totalCurrentCoins == totalMaxCoins) return true; 
        else return false; 
    }
}
