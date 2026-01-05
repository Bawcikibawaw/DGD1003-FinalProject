using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // 1. Enum to match your Enemy script
    public enum EnemyType { Runner, Tank, Normal, Ranged }

    [Header("Type Settings")]
    public EnemyType type;

    [Header("Movement Settings")]
    public float moveSpeed = 3f; 
    public float stoppingDistance = 5f; // Only Ranged enemies use this

    [Header("Components")]
    public Transform playerTarget; 
    private Rigidbody2D rb;
    private SpriteRenderer sr; 
    private TimeRewind timeRewind; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); 
        timeRewind = GetComponent<TimeRewind>(); 

        if (rb != null) rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
    }

    void FixedUpdate()
    {
        // Don't move if rewinding time
        if (timeRewind != null && timeRewind.IsRewinding())
        {
            rb.linearVelocity = Vector2.zero; 
            return; 
        }

        if (playerTarget != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTarget.position);
            Vector2 direction = (playerTarget.position - transform.position).normalized;

            // 2. The Stop Logic
            // If it's Ranged and within distance, stop. Otherwise, keep moving.
            if (type == EnemyType.Ranged && distanceToPlayer <= stoppingDistance)
            {
                rb.linearVelocity = Vector2.zero; 
            }
            else
            {
                rb.linearVelocity = direction * moveSpeed;
            }

            // 3. Flip Sprite based on direction to Player
            if (direction.x > 0) sr.flipX = false; 
            else if (direction.x < 0) sr.flipX = true; 
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void LateUpdate()
    {
        // Fix for rotation if any animation tries to rotate the enemy
        transform.rotation = Quaternion.identity;
    }
}