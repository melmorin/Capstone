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
    private GameObject endScreen; 
    private TextMeshProUGUI endTextUI; 
    private GameManager gameManager; 
    private SpriteRenderer renderer; 

    // Start is called before the first frame update
    void Start()
    {
        endScreen = GameObject.Find("endMenu");
        endTextUI = GameObject.Find("endText").GetComponent<TextMeshProUGUI>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        renderer = GetComponent<SpriteRenderer>(); 
        renderer.sprite = sprite; 
        StartCoroutine(BobItem()); 
    }

    // Animates the item
    private IEnumerator BobItem()
    {
        while(!gameManager.gameOver)
        {
            transform.Translate(new Vector2(0, -.5f));
            yield return new WaitForSeconds(.1f);
            transform.Translate(new Vector2(0, .5f));
            yield return new WaitForSeconds(.1f);
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
        }
    }
}
