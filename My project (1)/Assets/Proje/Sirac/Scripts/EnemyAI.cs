using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f; 

    // Ghost script'i için public bıraktık
    public Transform playerTarget; 

    private Rigidbody2D rb;
    private SpriteRenderer sr; // YENİ: Resmi çevirmek için lazım
    private TimeRewind timeRewind; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>(); // SpriteRenderer'ı al
        timeRewind = GetComponent<TimeRewind>(); 

        // 1. DÖNMEYİ FİZİKSEL OLARAK KİLİTLE
        if (rb != null) rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
    }

    void FixedUpdate()
    {
        if (timeRewind != null && timeRewind.IsRewinding())
        {
            rb.linearVelocity = Vector2.zero; 
            return; 
        }

        if (playerTarget != null)
        {
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            // --- ESKİ HATALI KOD (SİLİNDİ) ---
            // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            // rb.MoveRotation(angle); 
            // ---------------------------------

            // --- YENİ DOĞRU KOD (SADECE RESMİ ÇEVİR) ---
            if (direction.x > 0)
            {
                sr.flipX = false; // Sağa bak
            }
            else if (direction.x < 0)
            {
                sr.flipX = true; // Sola bak (Aynala)
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // --- ZORLA DÜZeltme (GARANTİ ÇÖZÜM) ---
    void LateUpdate()
    {
        // Eğer başka bir script veya animasyon döndürmeye çalışırsa engelle
        transform.rotation = Quaternion.identity;
    }
}