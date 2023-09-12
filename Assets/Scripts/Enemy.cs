using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int maxHealth = 10; 
    private int currentHealth;
    [SerializeField] private Slider healthSlider; 
    [SerializeField] private Image fillImage;
    private float fillValue; 

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; 
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; 
        fillValue = maxHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
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
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Destroy(gameObject); 
    }

    // Update the UI healthbar 
    public void UpdateHealth(int amount)
    {
        healthSlider.gameObject.SetActive(true);
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

}
