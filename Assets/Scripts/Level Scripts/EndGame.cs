using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGame : MonoBehaviour
{
    [Header ("Dependancies")]
    [SerializeField] Sprite sprite; 
    [SerializeField] string endText; 
    [SerializeField] private GameObject endScreen; 
    [SerializeField] private TextMeshProUGUI endTextUI; 
    private GameManager gameManager; 
    private CreateParticle makeParticle;
    private SpriteRenderer render; 
    private SceneController sceneManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        makeParticle = GameObject.Find("GameManager").GetComponent<CreateParticle>();
        
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>(); 
        render = GetComponent<SpriteRenderer>(); 
        render.sprite = sprite; 
        StartCoroutine(BobItem()); 
    }

    // Animates the item
    private IEnumerator BobItem()
    {
        while(true)
        {
            transform.Translate(new Vector2(0, -.05f));
            yield return new WaitForSeconds(.75f);
            transform.Translate(new Vector2(0, .05f));
            yield return new WaitForSeconds(.75f);
        }
    }

    // When something collides with the object 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            endTextUI.text = endText; 
            endScreen.SetActive(true); 
            gameManager.gameOver = true; 
            makeParticle.MakeParticle(transform.position, gameObject);
            sceneManager.LevelWin(); 
            Destroy(gameObject);  
        }
    }
}
