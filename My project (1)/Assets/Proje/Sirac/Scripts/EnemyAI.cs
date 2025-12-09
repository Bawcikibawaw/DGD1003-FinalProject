using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f; 

    private Rigidbody2D rb;
    private Transform playerTarget; // Oyuncunun pozisyonunu saklayacak
    private TimeRewind timeRewind; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeRewind = GetComponent<TimeRewind>(); 

        // Sahnedeki "Player" tag'ine sahip objeyi bul ve hedefe ata
        // Bu objenin sürekli var olduğunu varsayıyoruz
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        // Zaman geri sarma kontrolü
        if (timeRewind != null && timeRewind.IsRewinding())
        {
            rb.linearVelocity = Vector2.zero; 
            return; 
        }

        // Oyuncu hedefi var mı diye kontrol et (her ihtimale karşı)
        if (playerTarget != null)
        {
            // --- BÜTÜN MANTIK BU KADAR ---
            // 'isChasing' kontrolü yok. Sadece hedefe doğru git.
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            // (İsteğe bağlı) Düşmanın oyuncuya bakmasını sağla
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(angle);
        }
    }

    // --- GÖRÜŞ ALANI (TRIGGER) FONKSİYONLARI TAMAMEN SİLİNDİ ---
    // OnTriggerEnter2D yok
    // OnTriggerExit2D yok
}