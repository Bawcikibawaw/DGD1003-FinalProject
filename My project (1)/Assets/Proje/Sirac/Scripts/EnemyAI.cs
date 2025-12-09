using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f; 

    private Rigidbody2D rb;
    
    // --- DÜZELTME BURADA ---
    // Başındaki 'private' (veya boşluk) yerine 'public' yazdık.
    // Artık Ghost script'i buna erişip hafızayı silebilir.
    public Transform playerTarget; 
    // -----------------------

    private TimeRewind timeRewind; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeRewind = GetComponent<TimeRewind>(); 

        // Sahnedeki "Player" etiketli objeyi bul ve ona kilitlen
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
    }

    void FixedUpdate()
    {
        // Zaman geri sarma kontrolü
        if (timeRewind != null && timeRewind.IsRewinding())
        {
            rb.linearVelocity = Vector2.zero; 
            return; 
        }

        // Hedefe doğru git (Sadece hedef varsa!)
        if (playerTarget != null)
        {
            Vector2 direction = (playerTarget.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;

            // Düşmanı oyuncuya döndür
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            rb.MoveRotation(angle);
        }
        else
        {
            // Hedef yoksa (Ghost mode açıldıysa) dur
            rb.linearVelocity = Vector2.zero;
        }
    }
}