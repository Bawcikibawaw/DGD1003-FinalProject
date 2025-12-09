using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;    
    public float lifeTime = 2f;  
    public int chargeAmount = 10; // Her düşman %10 doldursun (10 düşmanda dolar)

    private Rigidbody2D rb;
    private TimeRewind timeRewind; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeRewind = GetComponent<TimeRewind>();
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
            rb.linearVelocity = transform.up * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (timeRewind != null && timeRewind.IsRewinding()) return;

        if (other.CompareTag("Enemy"))
        {
            // --- YENİ KISIM ---
            // Oyuncuyu bul ve barını doldur
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerMovement pm = player.GetComponent<PlayerMovement>();
                if (pm != null)
                {
                    pm.AddCheatCharge(chargeAmount);
                }
            }
            // ------------------

            Destroy(other.gameObject); 
            Destroy(gameObject);       
        }
    }
}