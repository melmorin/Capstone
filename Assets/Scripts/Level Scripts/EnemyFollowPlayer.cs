using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float lineOfSite = 10;
    private Enemy enemyScript; 
    private Transform player; 
    private EnemyShoot enemyShoot; 
    private bool canMove = true; 
    private GameManager gameManager; 
    bool facingRight = false; 

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = GetComponent<Enemy>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyShoot = GetComponent<EnemyShoot>(); 
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameManager.gameOver && !enemyScript.dead)
        {
            float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

            float vectorDis = (player.position - transform.position).x; 

            if (vectorDis < 0 && facingRight)
            {
                Flip(); 
            }
            else if (vectorDis > 0 && !facingRight)
            {
                Flip();
            }

            if (enemyShoot != null)
            {
                canMove = enemyShoot.canMove;
            } 

            if (distanceFromPlayer < lineOfSite && canMove)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, player.position, moveSpeed * Time.deltaTime);
            }
        }
    }

    // Flips the enemy depending on the movement direction
    private void Flip()
    {
        facingRight = !facingRight; 
        transform.Rotate(0f, 180f, 0f);
    }

    // Draws the enemies field of view in scene editor 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; 
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
    }
}
