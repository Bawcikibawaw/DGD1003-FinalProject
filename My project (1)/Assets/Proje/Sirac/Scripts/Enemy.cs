using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    // Define the different enemy behaviors
    public enum EnemyType { Runner, Tank, Normal, Ranged }

    [Header("Type Settings")]
    public EnemyType type; // Select this in the Unity Inspector

    [Header("Ranged Settings (Only for Ranged Type)")]
    public GameObject projectilePrefab; 
    public Transform shootPoint;        
    public float fireForce = 12f;
    public float detectionRange = 7f;   // How far away it starts shooting

    [Header("Can Ayarları")]
    private int currentHealth;

    [Header("Saldırı Ayarları")]
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    [Header("Loot & XP")]
    public GameObject lootPrefab;
    [Range(0, 100)] public int dropChance = 20;
    public float xpAmount = 20f;
    public float cheatValueOnDeath = 10f;

    [Header("UI & Görsel")]
    public Slider healthBar;
    private SpriteRenderer sr;
    private Color originalColor;

    [Header("Efektler")]
    public GameObject damagePopupPrefab;
    public AudioClip hitSound;

    // Components
    private Animator anim;
    private Rigidbody2D rb;
    private EnemyMovement movementScript;
    private bool isDead = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<EnemyMovement>();

        // 1. Initialize stats based on the Enum
        SetupEnemyStats();

        if (sr != null) originalColor = sr.color;

        if (healthBar != null)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = currentHealth;
            healthBar.value = currentHealth;
        }
    }

    void Update()
    {
        if (isDead) return;

        // 2. Logic for Ranged Attack (Distance based)
        if (type == EnemyType.Ranged && PlayerMovement.Instance != null)
        {
            float distance = Vector2.Distance(transform.position, PlayerMovement.Instance.transform.position);
            if (distance <= detectionRange && Time.time > lastAttackTime + attackCooldown)
            {
                Attack(PlayerMovement.Instance.gameObject);
            }
        }

        // Animation Speed
        if (anim != null && rb != null)
        {
            float speed = rb.linearVelocity.magnitude;
            anim.SetFloat("Speed", speed);
        }
    }

    // --- ENUM CONFIGURATION ---
    void SetupEnemyStats()
    {
        switch (type)
        {
            case EnemyType.Runner:
                SetupRunner();
                break;
            case EnemyType.Tank:
                SetupTank();
                break;
            case EnemyType.Normal:
                SetupNormal();
                break;
            case EnemyType.Ranged:
                SetupRanged();
                break;
        }
    }

    void SetupRunner()
    {
        transform.localScale = new Vector3(2.5f, 2.5f, 1f);
        if (movementScript != null) movementScript.moveSpeed = 5f;
        currentHealth = 60;
        if (sr != null) sr.color = new Color(0.5f, 1f, 0.5f); // Green tint
        xpAmount = 15f;
    }

    void SetupTank()
    {
        transform.localScale = new Vector3(4.5f, 4.5f, 1f);
        if (movementScript != null) movementScript.moveSpeed = 1.5f;
        currentHealth = 400;
        if (sr != null) sr.color = new Color(1f, 0.5f, 0.5f); // Red tint
        xpAmount = 50f;
    }

    void SetupNormal()
    {
        transform.localScale = new Vector3(3f, 3f, 1f);
        if (movementScript != null) movementScript.moveSpeed = 3f;
        currentHealth = 150;
        if (sr != null) sr.color = Color.white;
        xpAmount = 25f;
    }

    void SetupRanged()
    {
        transform.localScale = new Vector3(2.8f, 2.8f, 1f);
        if (movementScript != null) movementScript.moveSpeed = 2.5f;
        currentHealth = 90;
        if (sr != null) sr.color = new Color(0.7f, 0.7f, 1f); // Blue tint
        xpAmount = 35f;
    }

    // --- ATTACK LOGIC ---
    void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        // Ranged enemies usually don't use collision damage, but you can remove this check if you want both
        if (type == EnemyType.Ranged) return; 

        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack(collision.gameObject);
            }
        }
    }

    void Attack(GameObject playerObj)
    {
        lastAttackTime = Time.time;

        // Determine attack type based on Enum
        switch (type)
        {
            case EnemyType.Ranged:
                ExecuteRangedAttack(playerObj);
                break;
            default:
                ExecuteMeleeAttack(playerObj);
                break;
        }
    }

    void ExecuteMeleeAttack(GameObject playerObj)
    {
        if (anim != null) anim.SetTrigger("Attack");

        PlayerMovement playerScript = playerObj.GetComponent<PlayerMovement>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
        }

        StartCoroutine(StopMovementBriefly());
    }

    void ExecuteRangedAttack(GameObject playerObj)
    {
        if (anim != null) anim.SetTrigger("Attack");

        if (projectilePrefab != null && shootPoint != null)
        {
            // Spawn projectile
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
            
            // Aim at player
            Vector2 direction = (playerObj.transform.position - shootPoint.position).normalized;
            
            // Launch
            Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
            if (projRb != null)
            {
                projRb.linearVelocity = direction * fireForce;
            }
        }
    }

    IEnumerator StopMovementBriefly()
    {
        if (movementScript != null) movementScript.enabled = false;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        if (!isDead && movementScript != null) movementScript.enabled = true;
    }

    // --- DAMAGE & DEATH ---
    public void TakeDamage(int damage, bool isCritical)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (healthBar != null) healthBar.value = currentHealth;
        if (sr != null) StartCoroutine(FlashEffect());
        if (anim != null) anim.SetTrigger("Hurt");

        if (damagePopupPrefab != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(damage, isCritical);
        }

        if (hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position);

        if (currentHealth <= 0) Die();
    }

    public void TakeDamage(int damage) { TakeDamage(damage, false); }

    IEnumerator FlashEffect()
    {
        sr.color = new Color(1f, 0.2f, 0.2f); 
        yield return new WaitForSeconds(0.1f); 
        sr.color = originalColor; 
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (rb != null) rb.linearVelocity = Vector2.zero;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        if (movementScript != null) movementScript.enabled = false;

        if (anim != null) anim.SetTrigger("Die");

        if (LevelSystem.instance != null)
        {
            LevelSystem.instance.AddExperience(xpAmount);
            LevelSystem.instance.AddCheatValue(cheatValueOnDeath);
        }

        if (lootPrefab != null && Random.Range(0, 100) <= dropChance)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }

        if (PlayerMovement.Instance != null) PlayerMovement.Instance.AddCheatCharge(5);

        Destroy(gameObject, 1f); 
    }
}