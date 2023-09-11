using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rigb; 
    private BoxCollider2D coll; 

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Image fillImage;
    [SerializeField] private Slider healthSlider; 
    private float fillValue; 

    [SerializeField] private float velocity = 14f; 
    [SerializeField] private float speed = 7f; 
    private float dirX; 
    [SerializeField] private int maxHealth = 3; 
    [SerializeField] private int currentHealth; 

    [SerializeField] private Transform attackPoint; 
    [SerializeField] private float attackRange = 1.25f;
    [SerializeField] private LayerMask enemyLayers; 
    [SerializeField] private int attackDamage = 5; 
    [SerializeField] private float attackRate = 2f; 
    private float nextAttack = 0f; 

    private bool facingRight = true; 

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth; 
        rigb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>(); 
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; 
        fillValue = maxHealth; 
    }

    // Update is called once per frame
    private void Update()
    {
        // Movement and Jump
        dirX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && OnGround())
        { 
            rigb.velocity = new Vector2(rigb.velocity.x, velocity); 
        }

        if (Time.time >= nextAttack)
        {
            // Melee attack
            if (Input.GetButtonDown("Fire1"))
            {
                MeleeAttack();
                nextAttack = Time.time + 1f / attackRate; 
            }
        }
    }  

    // Function for a Melee attack
    private void MeleeAttack()
    {
        //Play animation when created 
        Collider2D[] hitenemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers); 

        foreach(Collider2D enemy in hitenemies)
        { 
            enemy.transform.parent.GetComponent<Enemy>().TakeDamage(attackDamage); 
        }
    }

    // Draw sphere in editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return; 
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    // Moves the player at a fixed rate 
    private void FixedUpdate()
    {
        rigb.velocity = new Vector2(dirX * speed, rigb.velocity.y);

        if (dirX < 0 && facingRight)
        {
            flip();
        }
        else if (dirX > 0 && !facingRight)
        {
            flip();
        }
    }

    // Flips the player depending on the movement direction
    private void flip()
    {
        facingRight = !facingRight; 
        transform.Rotate(0f, 180f, 0f);
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
        UpdateHealth(amount); 

        if (currentHealth <= 0){
            PlayerDeath();
        }
    }
    
    // Everything that happens when the player dies
    private void PlayerDeath()
    {
        Destroy(gameObject); 
        Debug.Log("You died!");
    }

    // Update the UI healthbar 
    public void UpdateHealth(int amount)
    {
        fillValue -= amount; 

        healthSlider.value = fillValue; 

        if (healthSlider.value <= healthSlider.minValue)
        {
            fillImage.enabled = false; 
        }
        if (healthSlider.value > healthSlider.minValue && !fillImage.enabled)
        {
            fillImage.enabled = true; 
        }

        if (healthSlider.value <= healthSlider.maxValue / 3)
        {
            fillImage.color = Color.red;
        }
    }

}
