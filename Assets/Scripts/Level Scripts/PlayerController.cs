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
    [SerializeField] private Transform attackPoint; 
    [SerializeField] private GunRotation firePoint; 
    [SerializeField] private GameObject bulletPrefab;

    [Header ("Player Settings")]
    [SerializeField] private float velocity = 14f; 
    [SerializeField] private float speed = 7f; 
    public int maxHealth = 10; 
    [SerializeField] private float flashTime = .5f; 
    [SerializeField] private float dashingPower = 24f; 
    [SerializeField] private float dashingTime = 0.2f; 
    [SerializeField] private float dashingCooldown = 1f; 

    [Header ("Melee Settings")]
    [SerializeField] private float attackRange = .5f;
    [SerializeField] private int attackDamage = 10; 

    [Header ("Range Settings")]
    [SerializeField] private float chargeSpeed = .04f; 
    [SerializeField] private float chargeInterval = .005f; 
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float shootDelay = .25f;  

    [Header ("Runtime Vars")]
    public float bulletSpeed; 
    public bool isCharging = false; 
    public float rotZ; 
    public int currentHealth; 

    // Private Components 
    private Rigidbody2D rigb; 
    private BoxCollider2D coll; 
    private GameManager gameManager; 
    private Animator anim; 
    private GameObject bullet; 
    private SpriteRenderer sprite;
    private ParticleSystem tr;  
    private ParticleSystem jumpParticle;  
    private ParticleSystem weaponParticle; 
    private CreateParticle particleScript;
    private SceneController sceneManager;

    // Private bools 
    private bool facingRight = true; 
    private bool canDash = true; 
    private bool isDashing;
    private bool invincibility; 
    private bool dead = false; 
    private bool inPlatform = false; 
    private bool hasCollided;
    private bool meleeAttacking; 
    private bool startDelay; 
    private bool jumping; 

    // Private float 
    private float fillValue; 
    private float dirX; 

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth; 
        rigb = GetComponent<Rigidbody2D>();
        tr = GameObject.Find("TrailParticle").GetComponent<ParticleSystem>();
        jumpParticle = GameObject.Find("FeetParticle").GetComponent<ParticleSystem>();
        weaponParticle = GameObject.Find("WeaponParticle").GetComponent<ParticleSystem>();
        coll = GetComponent<BoxCollider2D>(); 
        anim = GetComponent<Animator>(); 
        sprite = GetComponent<SpriteRenderer>(); 
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>();
        particleScript = GameObject.Find("GameManager").GetComponent<CreateParticle>(); 
        sceneManager = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<SceneController>(); 
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; 
        fillValue = maxHealth; 

        StartCoroutine(StartDelay()); 
    }

    // All delays that run at start 
    private IEnumerator StartDelay()
    {
        startDelay = true;
        yield return new WaitForSeconds(1f); 
        startDelay = false; 
    }
    
    // Update is called once per frame
    private void Update()
    {

        if (!dead && !gameManager.gameOver && !gameManager.gameIsPaused && !sceneManager.isLoading)
        {
            // Movement
            dirX = Input.GetAxisRaw("Horizontal");
            anim.SetFloat("speed", Mathf.Abs(dirX)); 

            // firepoint rotation 
            anim.SetFloat("rotZ", rotZ);


            // Dash 
            if (Input.GetButtonDown("Fire3") && canDash && !meleeAttacking && !isCharging)
            {
                StartCoroutine(Dash());
            }

            // Melee attack
            else if (Input.GetButtonDown("Fire1") && !meleeAttacking && !isCharging && OnGround() && !startDelay)
            {
                rigb.velocity = new Vector2(0,0); 
                meleeAttacking = true; 
                anim.SetBool("melee", true); 
            }

            // Ranged Attack 
            else if (Input.GetButtonDown("Fire2") && !isCharging && !meleeAttacking && OnGround() && !startDelay)
            {
                rigb.velocity = new Vector2(0,0); 
                isCharging = true; 
                StartCoroutine(Shoot()); 
            }

            // Jump 
            else if (Input.GetButtonDown("Jump"))
            { 
                if (OnGround() && !jumping && !isDashing && !inPlatform && !meleeAttacking && !isCharging)
                {
                    hasCollided = false; 
                    jumping = true;
                    StartCoroutine(PlayParticle(.2f, jumpParticle));  
                    StartJump(); 
                    rigb.velocity = new Vector2(rigb.velocity.x, velocity);  
                }
            }
        }

        else if (gameManager.gameOver && !dead)
        {
            anim.SetTrigger("win");
            anim.SetBool("airborn", true); 
            rigb.gravityScale = 15f; 
        }
    }  

    // Plays particle for certain time
    public IEnumerator PlayParticle(float time, ParticleSystem particle)
    {
        particle.Play();
        yield return new WaitForSeconds(time);
        particle.Stop(); 
    }

    // makes sure the player can actually die 
    private IEnumerator DeathCatch()
    {
        yield return new WaitForSeconds(8);
        anim.SetTrigger("Hit_Ground");
    }

    // Runs when melee animaiton ends 
    private void EndMelee()
    {
        meleeAttacking = false; 
        anim.SetBool("melee", false);
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
        anim.SetTrigger("jump");
    }

    // Moves the player at a fixed rate 
    private void FixedUpdate()
    {
        if (!gameManager.gameOver && !dead && !sceneManager.isLoading)
        {
            if (isDashing)
            {
                return; 
            } 

            if (!meleeAttacking && !isCharging)
            {
                rigb.velocity = new Vector2(dirX * speed, rigb.velocity.y);

                if (dirX < 0 && facingRight) 
                {
                    Flip();
                }
                else if (dirX > 0 && !facingRight)
                {
                    Flip();
                }
            }
        }

        else rigb.velocity = new Vector2(0, 0f);

        anim.SetFloat("velocity_y", rigb.velocity.y);
    }

    // Flips the player depending on the movement direction
    private void Flip()
    {
        facingRight = !facingRight; 
        transform.Rotate(0f, 180f, 0f);

        jumpParticle.gameObject.transform.Rotate(180f, 0f, 0f);
    }

    // Checks if the player is on the ground
    private bool OnGround()
    { 
        //return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);

        return Physics2D.BoxCast(coll.bounds.min, new Vector2(coll.bounds.size.x, .1f), 0f, Vector2.down, .1f, jumpableGround);
    }

    // Checks to see if the player enters a trigger 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameManager.gameOver && !dead)
        {
            if (collision.tag == "Enemy_Hitbox")
            {
                Enemy script = collision.GetComponentInParent<Enemy>();

                if (!script.dead)
                {
                    TakeDamage(2);
                }
            }
            else if (collision.tag == "Bullet")
            {
                TakeDamage(1);
            }
            else if (collision.tag == "Coin")
            {
                particleScript.MakeParticle(collision.transform.position, collision.gameObject); 
                Destroy(collision.gameObject); 
                gameManager.AddCoin(); 
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                TakeDamage(100);
            }
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

    // Runs particle from animation
    public void WalkingParticle()
    {
        StartCoroutine(PlayParticle(.1f, jumpParticle));
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

    // Checks to see if the player has hit the ground 
    private IEnumerator WhenGround()
    {
        anim.SetBool("airborn", true); 
        
        yield return new WaitForSeconds(.1f);

        bool loop = true; 
        while (loop)
        {
            yield return new WaitUntil(() => hasCollided);
            if (OnGround() && !inPlatform) loop = false; 
        }

        hasCollided = false; 
        anim.SetBool("airborn", false); 
        anim.ResetTrigger("jump");
        anim.SetTrigger("Hit_Ground");
        jumping = false; 
    }

    // Code that runs when the player has finished the hitground animation
    public void HitGround()
    {
        anim.ResetTrigger("Hit_Ground");
    }

    // Take Damage 
    public void TakeDamage(int amount)
    {
        if (currentHealth == maxHealth && amount < 0)
        {
            return;
        }

        else if (!invincibility || amount == 100)
        {
            currentHealth = currentHealth - amount;

            if (currentHealth > maxHealth) currentHealth = maxHealth; 

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
        anim.SetTrigger("die"); 
        dead = true; 
        StartCoroutine(DeathCatch()); 
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
        tr.Play(); 
        anim.SetTrigger("dash");
        
        float originalGravity = rigb.gravityScale;
        rigb.gravityScale = 0f; 

        if (!facingRight) rigb.velocity = new Vector2(transform.localScale.x * -dashingPower, 0f);

        else rigb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);

        yield return new WaitForSeconds(dashingTime);

        StartCoroutine(EndDash(originalGravity)); 
        
    }

    // End Dash
    private IEnumerator EndDash(float originalGravity)
    {  
        rigb.gravityScale = originalGravity; 
        isDashing = false; 

        anim.SetBool("isDashing", false);
        tr.Stop(); 

        if (OnGround() && !inPlatform) anim.SetBool("onGround", true);
        else anim.SetBool("onGround", false); 

        yield return new WaitForSeconds(dashingCooldown);

        canDash = true; 
    }

    // Shoots a projectile 
    private IEnumerator Shoot()
    {
        anim.SetBool("isCharging", true);
        anim.SetTrigger("shoot");
        bulletSpeed = 0; 
        while (Input.GetButton("Fire2") && bulletSpeed < maxSpeed)
        {
            bulletSpeed += chargeSpeed;
            yield return new WaitForSeconds(chargeInterval); 
        }     
        anim.SetBool("isCharging", false);
        bullet = Instantiate(bulletPrefab, firePoint.currentPoint.position, firePoint.currentPoint.rotation);
        yield return new WaitForSeconds(shootDelay); 
    }

    // Ends shooting animation
    private void EndShootAnim()
    {
        isCharging = false; 
    }

    // Plays Weapon Particle
    public void PlayWeaponParticle()
    {
        weaponParticle.Play(); 
    }

    // Stops Weapon Particle
    public void StopWeaponParticle()
    {
        weaponParticle.Stop(); 
    }
}
