using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private float speed; 
    
    private Rigidbody2D bulletRb; 
    private GameObject target; 

    // Start is called before the first frame update
    void Start()
    {
        bulletRb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        Vector2 moveDir = (target.transform.position - transform.position).normalized * speed; 
        bulletRb.velocity = new Vector2(moveDir.x, moveDir.y);
        Destroy(this.gameObject, 4); 
    }

    // If bullet hits player it disappears 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "Player_Bullet")
        {
            Destroy(gameObject);
        }
    }
}
