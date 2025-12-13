using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Mermi Ayarları")]
    public float speed = 20f;      
    public int damage = 15; // Hasarı 15 yaptık (Tek atmasın diye)
    public float lifetime = 3f;    

    [Header("Efektler")]
    public GameObject bloodPrefab; 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Merminin şekli dikey olduğu için 'up' yönüne gitsin
        rb.linearVelocity = transform.up * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // SADECE TAG KONTROLÜ
        if (hitInfo.CompareTag("Enemy"))
        {
            // 1. Kan Efekti
            if (bloodPrefab != null)
            {
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                Instantiate(bloodPrefab, hitInfo.transform.position, randomRotation);
            }

            // 2. KNOCKBACK (Geri İtme)
            Rigidbody2D enemyRb = hitInfo.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                 enemyRb.AddForce(transform.up * 400f, ForceMode2D.Impulse);
            }

            // 3. CAN AZALTMA (En Önemli Kısım Burası)
            // Önceki kodda direkt 'Destroy' vardı, şimdi script'e ulaşıyoruz.
            Enemy enemyScript = hitInfo.GetComponent<Enemy>();
            
            if (enemyScript != null)
            {
                // Can azaltma fonksiyonunu çağır (Bu fonksiyon konsola yazı yazdıracak)
                enemyScript.TakeDamage(damage); 
            }
            
            // Merminin kendisi yok olsun (Düşman değil!)
            Destroy(gameObject); 
        }
        else if (hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}