using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float speed = 3f;
    public float stoppingDistance = 6f;
    public float retreatDistance = 3.5f;

    [Header("Savaş Ayarları")]
    public float startTimeBetweenShots = 2f;
    private float timeBetweenShots;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Can Ayarları")]
    public int health = 30;

    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Start()
    {
        if (PlayerMovement.Instance != null)
            player = PlayerMovement.Instance.transform;
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        timeBetweenShots = startTimeBetweenShots;
    }

    void Update()
    {
        if (player == null) return;

        // --- HAREKET ---
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool isMoving = false;

        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            isMoving = true;
        }
        else if (distanceToPlayer < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            isMoving = true;
        }
        else
        {
            transform.position = this.transform.position;
            isMoving = false;
        }

        if (animator != null) animator.SetBool("IsMoving", isMoving);

        // --- YÖN (SADECE AYNALAMA - DÖNME YOK) ---
        if (player.position.x < transform.position.x)
            spriteRenderer.flipX = true; // Sola bak
        else
            spriteRenderer.flipX = false; // Sağa bak

        // --- ATEŞ ETME ---
        if (timeBetweenShots <= 0)
        {
            if (animator != null) animator.SetTrigger("Attack");
            Shoot();
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    // --- KESİN ÇÖZÜM BURASI (LateUpdate) ---
    void LateUpdate()
    {
        // Her şey bittikten sonra açıyı ZORLA SIFIRLA (Dik dur)
        transform.rotation = Quaternion.identity;
    }

    void Shoot()
    {
        if (projectilePrefab != null)
        {
            Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
            Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}