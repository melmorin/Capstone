using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [Header ("Dependancies")]
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private LayerMask enemyLayers; 
    [SerializeField] private Image fillImage;
    [SerializeField] private Slider healthSlider; 
    [SerializeField] private TrailRenderer tr; 
    [SerializeField] private Transform attackPoint; 
    [SerializeField] private Transform firePoint; 
    [SerializeField] private GameObject bulletPrefab;

    [Header ("Player Settings")]
    [SerializeField] private float velocity = 14f; 
    [SerializeField] private float speed = 7f; 
    [SerializeField] private int maxHealth = 10; 
    [SerializeField] private float flashTime = .5f; 
    [SerializeField] private float dashingPower = 24f; 
    [SerializeField] private float dashingTime = 0.2f; 
    [SerializeField] private float dashingCooldown = 1f; 

    [Header ("Melee Settings")]
    [SerializeField] private float attackRange = 1.25f;
    [SerializeField] private float attackRate = 2f; 
    [SerializeField] private int attackDamage = 3; 

    [Header ("Range Settings")]
    [SerializeField] private float chargeSpeed = .04f; 
    [SerializeField] private float chargeInterval = .005f; 
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float shootDelay = .25f;  

    [Header ("Runtime Vars")]
    public float bulletSpeed; 
   
    // Private Ints
    private int currentHealth; 

    // Private Components 
    private Rigidbody2D rigb; 
    private BoxCollider2D coll; 
    private GameManager gameManager; 
    private Animator anim; 
    private GameObject bullet; 
    private SpriteRenderer sprite; 

    // Private bools 
    private bool facingRight = true; 
    private bool canDash = true; 
    private bool isDashing;
    private bool isCharging = false; 
    private bool invincibility; 
    private bool dead = false; 
    private bool inPlatform = false; 
    private bool hasCollided;

    // Private float 
    private float fillValue; 
    private float dirX; 
    private float nextAttack = 0f; 

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth; 
        rigb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>(); 
        anim = GetComponent<Animator>(); 
        sprite = GetComponent<SpriteRenderer>(); 
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; 
        fillValue = maxHealth; 
    }

    // Update is called once per frame
    private void Update()
    {
        if (!dead)
        {
            // Movement
        dirX = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("speed", Mathf.Abs(dirX)); 

        // Jump 
        if (Input.GetButtonDown("Jump"))
        { 
            if (OnGround() && !isDashing && !inPlatform)
            {
                hasCollided = false; 
                StartJump(); 
                rigb.velocity = new Vector2(rigb.velocity.x, velocity);  
            }
        
        }

        // Dash 
        if (Input.GetButtonDown("Fire3") && canDash)
        {
            StartCoroutine(Dash());
        }

        // Melee attack
        if (Time.time >= nextAttack)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                MeleeAttack();
                nextAttack = Time.time + 1f / attackRate; 
            }
        }

        // Ranged Attack 
        if (Input.GetButtonDown("Fire2") && !isCharging)
        {
            StartCoroutine(Shoot()); 
        }
        }
    }  

    // Function for a Melee attack
    private void MeleeAttack()
    {
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

    // Sets Jump bool to true
    private void StartJump()
    {
        anim.SetBool("Jumping", true); 
    }

    // Moves the player at a fixed rate 
    private void FixedUpdate()
    {
        if (!gameManager.gameOver)
        {
            if (isDashing)
            {
                return; 
            } 

            rigb.velocity = new Vector2(dirX * speed, rigb.velocity.y);
            anim.SetFloat("velocity_y", rigb.velocity.y);

            if (dirX < 0 && facingRight) 
            {
                Flip();
            }
            else if (dirX > 0 && !facingRight)
            {
                Flip();
            }
        }
        else rigb.velocity = new Vector2(0, 0f);
    }

    // Flips the player depending on the movement direction
    private void Flip()
    {
        facingRight = !facingRight; 
        transform.Rotate(0f, 180f, 0f);
    }

    // Checks if the player is on the ground
    private bool OnGround()
    { 
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    // Checks to see if the player enters a trigger 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 impulse = new Vector2(-7, 2);
        if (collision.tag == "Enemy_Hitbox")
        {
            TakeDamage(2);
        }
        else if (collision.tag == "Bullet")
        {
            TakeDamage(1);
        }
    }

    // Runs when the player exits a collider 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            Vector3 hit = collision.contacts[0].normal;
            float angle = Vector3.Angle(hit, Vector3.up);

            if(Mathf.Approximately(angle, 180) || Mathf.Approximately(angle, 90))
            {
                inPlatform = true; 
            }
        }

        hasCollided = true; 
    }

    // Runs when the player exits a collider 
    private void OnCollisionExit2D(Collision2D collision)
    {        
        if (collision.gameObject.tag == "Platform")
        {          
            inPlatform = false;  
        }
        hasCollided = false; 
    }

    // Code that runs when the jump animation has ended 
    public void JumpOver()
    {
        anim.SetBool("Jumping", false);
    }


    // Checks to see if the player has hit the ground 
    private IEnumerator WhenGround()
    {
        anim.SetBool("Jumping", true);
        yield return new WaitForSeconds(.1f);

        bool loop = true; 
        while (loop)
        {
            yield return new WaitUntil(() => hasCollided);
            if (OnGround() && !inPlatform) loop = false; 
        }

        hasCollided = false; 

        anim.SetBool("Jumping", false);
        anim.SetBool("Hit_Ground", true);
    }

    // Code that runs when the player has finished the hitground animation
    public void HitGround()
    {
        anim.SetBool("Hit_Ground", false);
    }

    // Take Damage 
    public void TakeDamage(int amount)
    {
        if (currentHealth == maxHealth && amount < 0)
        {
            return;
        }

        else if (!invincibility)
        {
            currentHealth = currentHealth - amount;
            UpdateHealth(amount); 
            StartCoroutine(FlashColor(amount)); 
        }

        if (currentHealth <= 0){
            PlayerDeath();
        }
    }
    
    // Everything that happens when the player dies
    private void PlayerDeath()
    {
        anim.SetBool("Dying", true); 
        dead = true; 
        gameManager.Death(); 
        rigb.gravityScale = 15f; 
    }

    // Ends the game after death animation
    private void EndGame()
    {
        anim.speed = 0; 
        gameManager.EndGame();
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
        else { 
            fillImage.color = Color.green;
        }       
    }

    private IEnumerator FlashColor(int amount)
    {
        if (amount > 0) invincibility = true; 

        if (amount > 0)
        {
            if (currentHealth > 0)
            {
                sprite.color = new Color(1,0,0); 
                yield return new WaitForSeconds(flashTime);
                sprite.color = new Color(1,1,1);
            }
        }
        else { 
            if (currentHealth > 0)
            {
                sprite.color = new Color(0,1,0); 
                yield return new WaitForSeconds(flashTime);
                sprite.color = new Color(1,1,1); 
            }
        }
        invincibility = false; 
    }

    // Dashes the player 
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true; 

        anim.SetBool("isDashing", true);
        float originalGravity = rigb.gravityScale;
        rigb.gravityScale = 0f; 

        if (!facingRight) rigb.velocity = new Vector2(transform.localScale.x * -dashingPower, 0f);

        else rigb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
       
        tr.emitting = true; 
        yield return new WaitForSeconds(dashingTime);

        StartCoroutine(EndDash(originalGravity)); 
        
    }

    // End Dash
    private IEnumerator EndDash(float originalGravity)
    {
        tr.emitting = false;
       
        rigb.gravityScale = originalGravity; 
        isDashing = false; 
        anim.SetBool("isDashing", false);

        yield return new WaitForSeconds(dashingCooldown);

        canDash = true; 
    }

    // Shoots a projectile 
    private IEnumerator Shoot()
    {
        bulletSpeed = 0; 
        isCharging = true; 
        while (Input.GetButton("Fire2") && bulletSpeed < maxSpeed)
        {
            bulletSpeed += chargeSpeed;
            yield return new WaitForSeconds(chargeInterval); 
        }     
        bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        yield return new WaitForSeconds(shootDelay); 
        isCharging = false; 
    }
}
