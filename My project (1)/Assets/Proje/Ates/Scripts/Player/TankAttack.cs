using UnityEngine;
using System.Collections;

public class TankAttack : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashSpeed = 25f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    [Header("Combat Settings")]
    public int dashDamage = 20;
    public string enemyTag = "Enemy";

    private Rigidbody2D rb;
    private Collider2D myCollider; // Reference to the player's collider
    private bool isDashing = false;
    private float lastDashTime;
    private Animator animator;

    public bool IsDashing => isDashing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        myCollider = GetComponent<Collider2D>(); // Get the collider component
        
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        Vector2 dashDirection = ((Vector2)mouseWorldPos - (Vector2)transform.position).normalized;

        StartCoroutine(DashRoutine(dashDirection));
    }

    IEnumerator DashRoutine(Vector2 direction)
    {
        isDashing = true;
        lastDashTime = Time.time;

        // 1. Enable Trigger mode so we pass through enemies
        if (myCollider != null) myCollider.isTrigger = true;

        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isDashing", true);
        
        if(PlayerMovement.Instance != null)
            PlayerMovement.Instance.isInvincible = true;

        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            rb.linearVelocity = direction * dashSpeed;
            yield return null; 
        }

        // 2. Disable Trigger mode back to normal physical collision
        if (myCollider != null) myCollider.isTrigger = false;

        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isDashing", false);
        
        if(PlayerMovement.Instance != null)
            PlayerMovement.Instance.isInvincible = false;
            
        isDashing = false;
    }

    // --- SWITCHED TO TRIGGER DETECTION ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if we are dashing and if the object we overlapped is an enemy
        if (isDashing && other.CompareTag(enemyTag))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(dashDamage);
                Debug.Log("Dashed THROUGH enemy and dealt " + dashDamage + " damage!");
            }
        }
    }
}