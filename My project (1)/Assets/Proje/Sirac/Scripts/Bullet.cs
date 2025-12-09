using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;    
    public float lifeTime = 2f;  

    private Rigidbody2D rb;
    private TimeRewind timeRewind; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeRewind = GetComponent<TimeRewind>();

        // Başlangıç hızı
        rb.linearVelocity = transform.up * speed;
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (timeRewind != null && timeRewind.IsRewinding())
        {
            rb.linearVelocity = Vector2.zero;
        }
        else 
        {
            // Geri sarma bittiğinde tekrar hareket etmesi için
            rb.linearVelocity = transform.up * speed;
        }
    }

  void OnTriggerEnter2D(Collider2D other)
    {
        // --- BU SATIRI EKLE ---
        Debug.Log("Mermi şuna çarptı: " + other.name); 
        // ----------------------

        if (timeRewind != null && timeRewind.IsRewinding()) return;

        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); 
            Destroy(gameObject);       
        }
    }   
}