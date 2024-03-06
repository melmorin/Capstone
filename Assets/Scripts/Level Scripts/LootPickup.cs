using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootPickup : MonoBehaviour
{

    private PlayerController player; 
    private CreateParticle particleScript;

    private void Start()
    {
        player = transform.parent.GetComponent<PlayerController>(); 
        particleScript = GetComponent<CreateParticle>(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Heart" && player.currentHealth != player.maxHealth)
        { 
            particleScript.MakeParticle(other.transform.position, other.gameObject); 
            Destroy(other.gameObject);
            player.TakeDamage(-2);
        }
    }

}
