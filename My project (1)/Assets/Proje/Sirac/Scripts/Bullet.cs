using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Temel Ayarlar")]
    public float speed = 20f;
    public int damage = 15;      
    public float lifetime = 3f;

    [Header("Görsel Efektler")]
    public GameObject hitEffectPrefab; // Vuruş efekti buraya
    // --------------------------------------------------

    [Header("Kritik Vuruş Sistemi")]
    [Range(0, 100)] public int critChance = 20; 
    public int critMultiplier = 2; 

    [Header("Fizik & His")]
    public float knockbackForce = 5f; 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Unity 6 kullanıyorsan linearVelocity, eski sürümse velocity kullan.
            // Kodunda linearVelocity olduğu için aynen bırakıyorum.
            rb.linearVelocity = transform.up * speed;
        }
        Destroy(gameObject, lifetime);
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
            // Kritik Hesapla
            bool isCritical = Random.Range(0, 100) < critChance;
            int finalDamage = isCritical ? damage * critMultiplier : damage;
            
            // Hasar Ver
            enemy.TakeDamage(finalDamage, isCritical);

            // Hit Marker Göster
            HitMarker marker = FindObjectOfType<HitMarker>();
            if (marker != null) marker.Show();

            // Geri Tepme (Knockback)
            Rigidbody2D enemyRb = hitInfo.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(transform.up * knockbackForce, ForceMode2D.Impulse);
            }

            SpawnHitEffect(); // Efekt çıkar
            Destroy(gameObject); // Mermiyi yok et
        }
        
        // 2. PATLAYAN FIÇIYA ÇARPARSA
        else if (hitInfo.GetComponent<PatlayanFici>() != null)
        {
            hitInfo.GetComponent<PatlayanFici>().TakeDamage(damage);
            SpawnHitEffect(); // Efekt çıkar
            Destroy(gameObject);
        }
        
        // 3. DUVARA ÇARPARSA (Tag'i "Wall" olmalı)
        else if (hitInfo.CompareTag("Wall"))
        {
            SpawnHitEffect(); // Efekt çıkar
            Destroy(gameObject);
        }
    }
}