using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        // BAŞLANGIÇTA ZORLA KİLİTLE
        if(rb != null) rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // Yön hesapla
        Vector2 direction = (player.position - transform.position).normalized;

        // Hareketi uygula
        rb.linearVelocity = direction * moveSpeed;

        // --- RESMİ SAĞA/SOLA ÇEVİR (Flip) ---
        if (direction.x > 0)
        {
            sr.flipX = false; // Sağa bak
        }
        else if (direction.x < 0)
        {
            sr.flipX = true; // Sola bak
        }
    }

    // --- İŞTE BU FONKSİYON DÖNMEYİ ENGELLER ---
    // LateUpdate, her karede en son çalışır ve yapılan tüm dönüşleri iptal eder.
    void LateUpdate()
    {
        // Dönüşü her karede mecburen SIFIRLA (0,0,0)
        transform.rotation = Quaternion.identity; 
    }
}