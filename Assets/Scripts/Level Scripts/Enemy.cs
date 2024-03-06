using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header ("Dependancies")]
    [SerializeField] private Slider healthSlider; 
    [SerializeField] private Image fillImage;

    [Header ("Settings")]
    [SerializeField] private int maxHealth = 10; 

    [Header ("Runtime Vars")]
    public bool dead = false; 

    private int currentHealth;
    private float fillValue; 
    private Animator anim; 
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; 
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth; 
        fillValue = maxHealth; 
        anim = GetComponent<Animator>(); 
        sprite = GetComponent<SpriteRenderer>(); 
    }

    // Damages the enemy when hit by the player 
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealth(damage); 
        StartCoroutine(FlashColor());
        StartCoroutine(PlayParticle()); 
        if (currentHealth <= 0)
        {
            dead = true;
            if (!gameObject.GetComponent<EnemyPatrol>())
            {
                SetDeadBool(); 
            }
        }
    }

    // Sets dead anim bool
    public void SetDeadBool()
    {
        if (dead)
        {
            anim.SetBool("dead", true); 
        }
    }

    // Plays the hit particle effect
    private IEnumerator PlayParticle()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(.2f);
        gameObject.GetComponent<ParticleSystem>().Stop();
    }

    // Performs all code related to enemy death
    private void EnemyDeath()
    {
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

    // Enemy flashed red when hit
    private IEnumerator FlashColor()
    {
        sprite.color = new Color(1,0,0); 
        yield return new WaitForSeconds(.2f);
        sprite.color = new Color(1,1,1);
    }

}
