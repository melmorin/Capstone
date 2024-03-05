using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPickup : MonoBehaviour
{

    private PlayerController player; 

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerController>(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Heart" && player.currentHealth != player.maxHealth)
        {
            Destroy(other.gameObject);
            player.TakeDamage(-2);
        }
    }

}
