using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [Header("Dependancies")]
    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject startPoint; 

    private Text levelText; 
    private GameObject levelMenu; 
    public bool facingRight = true; 

    // Start is called before the first frame update
    void Start()
    {
        player.transform.position = startPoint.transform.position; 
        levelText = GameObject.FindGameObjectWithTag("Level_Text").GetComponent<Text>(); 
        levelMenu = GameObject.FindGameObjectWithTag("Level_Menu"); 
    }

    // Flips the player depending on the movement direction
    public void Flip()
    {
        facingRight = !facingRight; 
        player.transform.Rotate(0f, 180f, 0f);
    }

    public void SetMenuActive(bool active)
    {
        levelMenu.SetActive(active); 
    }

    // Update is called once per frame
    void Update()
    {

    }
}
