using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float lineOfSite = 10;

    private Transform player; 
    private EnemyShoot enemyShoot; 
    private bool canMove = true; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyShoot = GetComponent<EnemyShoot>(); 
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (enemyShoot != null)
        {
            canMove = enemyShoot.canMove;
        } 

        if (distanceFromPlayer < lineOfSite && canMove)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    // Draws the enemies field of view in scene editor 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green; 
        Gizmos.DrawWireSphere(transform.position, lineOfSite);
    }
}
