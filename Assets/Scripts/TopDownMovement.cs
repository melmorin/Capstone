using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; 
    [SerializeField] private Rigidbody2D rigb;

    private Vector2 movement;

    // Update is called once per frame
    private void Update()
    {
        // Input 
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        // Movement 
        rigb.MovePosition(rigb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
