using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [Header ("Dependancies")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject bulletParent; 
    
    [Header ("Settings")]
    [SerializeField] private float shootingRange;
    [SerializeField] private float fireRate = 1; 

    [Header ("Runtime Vars")]
    private float nextFireTime;
    private Transform player; 
    public bool canMove; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position);

        if (distanceFromPlayer <= shootingRange)
        {
            canMove = false;  
        }
        else 
        { 
            canMove = true;  
        }

        if (distanceFromPlayer <= shootingRange && nextFireTime < Time.time)
        {
            Instantiate(bullet, bulletParent.transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate; 
        }
    }

    // Draws the circle that the enemy shoots from in the scene editor 
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
