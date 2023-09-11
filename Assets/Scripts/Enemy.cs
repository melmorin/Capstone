using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; 
    private Rigidbody2D rigb;

    [SerializeField] private int maxHealth = 5; 
    private int currentHealth;

    [SerializeField] private Slider healthSlider; 
    [SerializeField] private Image fillImage;
    private float fillValue; 

    // Start is called before the first frame update
    void Start()
    {
        rigb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; 
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; 
        fillValue = maxHealth; 
    }

    // Update is called once per frame
    void Update()
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

    // Damage player when collided with 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rigb.velocity.x)), transform.localScale.y);
        }
    }

    // Checks which way the enemy is facing 
    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon; 
    }

    // Damages the enemy when hit by the player 
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        UpdateHealth(damage); 
        // Damage Animation

        if (currentHealth <= 0)
        {
            EnemyDeath(); 
        }
    }

    // Performs all code related to enemy death
    private void EnemyDeath()
    {
        // Dealth Animation
        Debug.Log("Enemy Died");
        Destroy(gameObject); 
    }

    // Update the UI healthbar 
    public void UpdateHealth(int amount)
    {
        healthSlider.gameObject.SetActive(true);
        fillValue -= amount; 
        Debug.Log(fillValue);

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
