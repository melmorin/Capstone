using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] private float moveSpeed = 5f; 
    private Animator anim; 
    private Rigidbody2D rigb;
    private Vector2 lastDirection; 
    private bool facingLeft = false; 
    [SerializeField] private ParticleSystem particles; 
    private GameManager gameManager; 

    [Header ("Runtime Vars")]
    public Vector2 movement;

    // Start is called before the first frame 
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game_Manager").GetComponent<GameManager>(); 
        anim = GetComponent<Animator>(); 
        rigb = GetComponent<Rigidbody2D>(); 
    }

    // Update calls once per frame 
    private void Update()
    {
        if (!gameManager.gameIsPaused)
        {
            ProcessInputs();
            Animate(); 

            if (movement.x < 0 && !facingLeft || movement.x > 0 && facingLeft)
            {
                Flip();
            }
        }
    }

    // FixedUpdate is called on Intervals 
    private void FixedUpdate()
    {
        rigb.velocity = movement * moveSpeed; 
    }

    // Animates the player 
    private void Animate()
    {
        anim.SetFloat("speed_x", movement.x);
        anim.SetFloat("speed_y", movement.y);
        anim.SetFloat("speed_magnitude", movement.magnitude);
        anim.SetFloat("last_x", lastDirection.x);
        anim.SetFloat("last_y", lastDirection.y);
    }

    // Flips the player depending on the movement direction
    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; 
        transform.localScale = scale;
        facingLeft = !facingLeft; 
    }

    // Input Function 
    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if ((moveX == 0 & moveY == 0) && (movement.x !=0 || movement.y !=0))
        {
            lastDirection = movement; 
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();
    }

    // Plays movement particle
    public IEnumerator PlayParticle()
    {
        particles.Play(); 
        yield return new WaitForSeconds(.1f);
        particles.Stop(); 
    }

}