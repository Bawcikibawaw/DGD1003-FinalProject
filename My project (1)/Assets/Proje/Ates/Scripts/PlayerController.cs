using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5.0f; 

    private Rigidbody2D rb;
    private SpriteRenderer sr; 
    private Vector2 moveInput; 
    private float currentHorizontalInput; // Variable to store the horizontal input
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); 
        
        if (rb == null)
        {
            Debug.LogError("PlayerMovement requires a Rigidbody2D component!");
        }
        if (sr == null)
        {
            Debug.LogError("PlayerMovement requires a SpriteRenderer component for flipping!");
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void Update()
    {
        // 1. Get Input
        currentHorizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        // 2. Calculate Movement Vector
        moveInput = new Vector2(currentHorizontalInput, verticalInput).normalized;
        
        // 3. Handle Sprite Flipping (Called as a separate method)
        HandleFlipping();
    }
    
    
    private void HandleFlipping()
    {
        // Only attempt to flip if the SpriteRenderer exists and there is horizontal input
        if (sr != null && currentHorizontalInput != 0)
        {
            // If moving right (positive input), ensure the sprite is NOT flipped (false)
            if (currentHorizontalInput > 0)
            {
                sr.flipX = false;
            }
            // If moving left (negative input), flip the sprite (true)
            else if (currentHorizontalInput < 0)
            {
                sr.flipX = true;
            }
        }
    }
    
}