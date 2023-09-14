using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Image fillImage;
    [SerializeField] private Slider healthSlider; 
    [SerializeField] private Transform attackPoint; 
    [SerializeField] private LayerMask enemyLayers; 
    [SerializeField] private TrailRenderer tr; 

    [SerializeField] private float velocity = 14f; 
    [SerializeField] private float speed = 7f; 
    [SerializeField] private float attackRange = 1.25f;
    [SerializeField] private float attackRate = 2f; 

    [SerializeField] private int maxHealth = 10; 
    [SerializeField] private int currentHealth; 
    [SerializeField] private int attackDamage = 3; 

    private Rigidbody2D rigb; 
    private BoxCollider2D coll; 

    private bool facingRight = true; 
    private bool canDash = true; 
    private bool isDashing;

    private float fillValue; 
    private float dirX; 
    private float nextAttack = 0f; 
    private float dashingPower = 24f; 
    private float dashingTime = 0.2f; 
    private float dashingCooldown = 1f; 


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

        if (Input.GetButtonDown("Jump"))
        { 
            if (OnGround() && !isDashing)
            {
                rigb.velocity = new Vector2(rigb.velocity.x, velocity); 

            }
        }

        if (Input.GetButtonDown("Fire3") && canDash)
        {
            StartCoroutine(Dash());
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
        if (isDashing)
        {
            return; 
        } 

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
            TakeDamage(2);
        }
        else if (collision.tag == "Bullet")
        {
            TakeDamage(1);
        }
        else if (collision.tag == "Heart")
        {
            Destroy(collision.gameObject);
            TakeDamage(-2);
        }
    }

    // Take Damage 
    private void TakeDamage(int amount)
    {
        if (currentHealth == maxHealth && amount < 0)
        {
            return;
        }

        currentHealth = currentHealth - amount;

        UpdateHealth(amount); 

        if (currentHealth <= 0){
            PlayerDeath();
        }
    }
    
    // Everything that happens when the player dies
    private void PlayerDeath()
    {
        Destroy(gameObject); 
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
        else if (healthSlider.value <= healthSlider.maxValue / 1.5)
        {
            fillImage.color = Color.yellow;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true; 
        float originalGravity = rigb.gravityScale;
        rigb.gravityScale = 0f; 
        if (!facingRight) rigb.velocity = new Vector2(transform.localScale.x * -dashingPower, 0f);
        else rigb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true; 
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rigb.gravityScale = originalGravity; 
        isDashing = false; 
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true; 
    } 

}
