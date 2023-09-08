using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigb; 
    private BoxCollider2D coll; 

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private BoxCollider2D enemyColl;

    [SerializeField] private float velocity = 14f; 
    [SerializeField] private float speed = 7f; 
    private float dirX; 

    [SerializeField] private int maxHealth = 3; 
    [SerializeField] private int currentHealth; 

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth; 
        rigb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>(); 
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && OnGround())
        { 
            rigb.velocity = new Vector2(rigb.velocity.x, velocity); 
        }
    }  

    // Moves the player at a fixed rate 
    private void FixedUpdate()
    {
        rigb.velocity = new Vector2(dirX * speed, rigb.velocity.y);
    }

    // Checks if the player is on the ground
    private bool OnGround()
    { 
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy_Hitbox")
        {
            TakeDamage(1);
        }
    }

    // Take Damage 
    private void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("You have "+currentHealth+" hearts left");

        if (currentHealth <= 0){
            Destroy(gameObject); 
            Debug.Log("You died!");
        }
    }

}
