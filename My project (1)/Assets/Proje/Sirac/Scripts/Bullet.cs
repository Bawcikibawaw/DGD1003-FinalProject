using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Temel Ayarlar")]
    public float speed = 20f;
    public int damage = 15;      
    public float lifetime = 3f;  

    [Header("Kritik Vuruş Sistemi")]
    [Range(0, 100)] public int critChance = 20; // %20 Şans
    public int critMultiplier = 2; // Hasarı 2'ye katla

    [Header("Fizik & His")]
    public float knockbackForce = 5f; // Geri tepme gücü
    private float damageMultiplier = 1f; // Default multiplier is 1x

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Mermiyi ileri fırlat
        if (rb != null)
        {
            rb.linearVelocity = transform.up * speed;
        }
        // Ömrü dolunca yok et
        Destroy(gameObject, lifetime);
    }
    
    public void SetDamageMultiplier(float multiplier)
    {
        damageMultiplier = multiplier;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        RangedEnemy rangedEnemy = hitInfo.GetComponent<RangedEnemy>();
        
        if (enemy != null)
        {
            //Destroy(gameObject);
            
            bool isCritical = Random.Range(0, 100) < critChance;
            
            // Apply the multiplier to the base damage
            float boostedDamage = damage * damageMultiplier;
            int finalDamage = isCritical ? Mathf.RoundToInt(boostedDamage * critMultiplier) : Mathf.RoundToInt(boostedDamage);
        
            enemy.TakeDamage(finalDamage, isCritical);
        
            // C. VURUŞ İŞARETİ (HIT MARKER) GÖSTER [YENİ EKLENEN]
            HitMarker marker = FindObjectOfType<HitMarker>();
            if (marker != null)
            {
                marker.Show();
            }
        
            // D. GERİ TEPME (KNOCKBACK) UYGULA
            Rigidbody2D enemyRb = hitInfo.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(transform.up * knockbackForce, ForceMode2D.Impulse);
            }
            
            // Mermiyi yok et
            //Destroy(gameObject);
        }
        
        // 2. PATLAYAN FIÇIYA ÇARPARSA
        else if (hitInfo.GetComponent<PatlayanFici>() != null)
        {
            hitInfo.GetComponent<PatlayanFici>().TakeDamage(damage);
            Destroy(gameObject);
        }
        
        // 3. DUVARA ÇARPARSA
        else if (hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}