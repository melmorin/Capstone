using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; 

    private Rigidbody2D rigb;

    // Start is called before the first frame update
    void Start()
    {
       rigb = GetComponent<Rigidbody2D>();
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            transform.localScale = new Vector2(-(Mathf.Sign(rigb.velocity.x)), transform.localScale.y);
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > Mathf.Epsilon; 
    }

}
