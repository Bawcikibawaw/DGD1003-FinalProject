using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Temel Ayarlar")]
    public float speed = 20f;
    public int damage = 15;      
    public float lifetime = 3f;

    [Header("Görsel Efektler")]
    public GameObject hitEffectPrefab; // Vuruş efekti buraya

    // --- YENİ EKLENEN: Kovan Prefabı ---
    public GameObject shellPrefab;     // Kovan (Shell) prefabı buraya
    // -----------------------------------

    [Header("Kritik Vuruş Sistemi")]
    [Range(0, 100)] public int critChance = 20; 
    public int critMultiplier = 2; 

    [Header("Fizik & His")]
    public float knockbackForce = 5f; 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Mermiyi fırlat
        if (rb != null)
        {
            rb.linearVelocity = transform.up * speed;
        }

        // --- YENİ EKLENEN: Mermi başlar başlamaz kovanı oluştur ---
        SpawnShell();
        // ----------------------------------------------------------

        Destroy(gameObject, lifetime);
    }

    // Kovanı oluşturan fonksiyon
    void SpawnShell()
    {
        if (shellPrefab != null)
        {
            // Merminin olduğu yerde kovanı oluştur. 
            // Kovanın kendi içindeki 'Shell' scripti onu yana fırlatacak.
            Instantiate(shellPrefab, transform.position, transform.rotation);
        }
    }

    // --- Efekti oluşturan fonksiyon ---
    void SpawnHitEffect()
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. DÜŞMANA ÇARPARSA
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        
        if (enemy != null)
        {
            bool isCritical = Random.Range(0, 100) < critChance;
            int finalDamage = isCritical ? damage * critMultiplier : damage;
            
            enemy.TakeDamage(finalDamage, isCritical);

            HitMarker marker = FindObjectOfType<HitMarker>();
            if (marker != null) marker.Show();

            Rigidbody2D enemyRb = hitInfo.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(transform.up * knockbackForce, ForceMode2D.Impulse);
            }

            SpawnHitEffect();
            Destroy(gameObject);
        }
        
        // 2. PATLAYAN FIÇIYA ÇARPARSA
        else if (hitInfo.GetComponent<PatlayanFici>() != null)
        {
            hitInfo.GetComponent<PatlayanFici>().TakeDamage(damage);
            SpawnHitEffect();
            Destroy(gameObject);
        }
        
        // 3. DUVARA ÇARPARSA
        else if (hitInfo.CompareTag("Wall"))
        {
            SpawnHitEffect();
            Destroy(gameObject);
        }
    }
}