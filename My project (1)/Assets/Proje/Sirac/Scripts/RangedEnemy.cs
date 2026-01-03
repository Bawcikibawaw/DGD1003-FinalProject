using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float speed = 3f;
    public float stoppingDistance = 6f; // Oyuncuya ne kadar yaklaşsın?
    public float retreatDistance = 3.5f; // Oyuncu dibine girerse kaçacağı mesafe

    [Header("Savaş Ayarları")]
    public float startTimeBetweenShots = 2f; // Kaç saniyede bir ateş etsin?
    private float timeBetweenShots;

    public GameObject projectilePrefab; // Mermi Prefab'ı
    public Transform firePoint;         // Merminin çıkış noktası

    [Header("Can Ayarları")]
    public int health = 30;

    // --- BİLEŞENLER ---
    private Transform player;
    private SpriteRenderer spriteRenderer;
    private Animator animator; // Animasyon için değişkenimiz

    void Start()
    {
        if (PlayerMovement.Instance != null)
        {
            player = PlayerMovement.Instance.transform;
        }
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); // Animator bileşenini alıyoruz
        
        // Eğer Animator eklemeyi unuttuysan hata vermesin diye kontrol
        if (animator == null) Debug.LogWarning("DİKKAT: Düşmanda Animator bileşeni yok!");

        timeBetweenShots = startTimeBetweenShots;
    }

    void Update()
    {
        if (player == null) return;

        // --- 1. HAREKET MANTIĞI VE ANİMASYON ---
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool isMoving = false; // Başlangıçta hareket etmiyor varsayalım

        if (distanceToPlayer > stoppingDistance)
        {
            // Yaklaş
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            isMoving = true; // Hareket ediyor
        }
        else if (distanceToPlayer < retreatDistance)
        {
            // Kaç
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            isMoving = true; // Hareket ediyor
        }
        else
        {
            // Dur
            transform.position = this.transform.position;
            isMoving = false; // Duruyor
        }

        // Animatore sonucu gönder (Yürü veya Dur)
        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }

        // --- 2. YÖN DÖNME (Flip X) ---
        if (player.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true; // Sola bak
        }
        else
        {
            spriteRenderer.flipX = false; // Sağa bak
        }

        // --- 3. ATEŞ ETME ---
        if (timeBetweenShots <= 0)
        {
            // Animasyon tetikle
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            Shoot();
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        }
        else if (projectilePrefab != null)
        {
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }
    }

    // --- 4. HASAR ALMA ---
    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Burada da ölüm animasyonu çalıştırılabilir (animator.SetTrigger("Die"))
        Destroy(gameObject);
    }
}