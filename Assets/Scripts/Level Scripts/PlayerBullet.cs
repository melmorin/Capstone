using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{

    private PlayerController playerController; 
    private Rigidbody2D rigb; 

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); 
        rigb = GetComponent<Rigidbody2D>();
        rigb.velocity = transform.right * playerController.bulletSpeed; 
    }

    // If the bullet hits an enemy it gets destroyed 
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>(); 
        if (enemy != null)
            {
                enemy.TakeDamage(3);
                Destroy(gameObject); 
            }
    }

    // If the bullet hit's the ground it gets destroyed 
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject); 
    }
}
