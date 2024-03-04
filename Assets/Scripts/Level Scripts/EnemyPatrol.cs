using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private float moveSpeed = 1f; 
    private Rigidbody2D rigb;
    private GameManager gameManager; 
    private Animator anim; 

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
        rigb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameOver)
        {
            if (IsFacingRight())
            {
                rigb.velocity = new Vector2(moveSpeed, 0f);
            }
            else 
            {
                rigb.velocity = new Vector2(-moveSpeed, 0f);
            }
        }
    }

    // Ends Animation at certain point if game is over
    void EndAnim()
    {
        if (gameManager.gameOver)
        {
            anim.speed = 0; 
            rigb.velocity = new Vector2(0, 0f);
        }
    }

    // Move enemy when hitting an edge  
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rigb.velocity.x)), transform.localScale.y);
        }
    }

    // Checks which way the enemy is facing 
    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon; 
    }
}
