using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private float moveSpeed = 1f;
    private Rigidbody2D rigb;
    private GameManager gameManager; 
    private Enemy enemyScript; 
    private Animator anim; 
    private bool facingRight = true; 


    // Start is called before the first frame update
    void Start()
    {
        enemyScript = GetComponent<Enemy>();
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
        rigb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.gameOver && !enemyScript.dead)
        {
            if (facingRight)
            {
                rigb.velocity = new Vector2(moveSpeed, 0f);
            }
            else 
            {
                rigb.velocity = new Vector2(-moveSpeed, 0f);
            }
        }
        else 
        {
            rigb.velocity = new Vector2(0f, 0f); 
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
            if (transform.rotation.y == 0)
            {
                transform.Rotate(0, 180, 0);
                facingRight = false; 
            }
            else 
            {
                transform.Rotate(0, -180, 0);
                facingRight = true; 
            }
        }
    }
}
