using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;    // Mermi hızı
    public float lifeTime = 2f;  // Mermi 2 saniye sonra kendini yok etsin

    private Rigidbody2D rb;
    private TimeRewind timeRewind; // Kendi geri sarma bileşeni

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeRewind = GetComponent<TimeRewind>();

        // Mermiyi oluşturulduğu anda 'yukarı' (forward) yönde fırlat
        rb.linearVelocity = transform.up * speed;

        // Mermiyi 'lifeTime' saniye sonra yok et
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // Eğer zaman geri sarılıyorsa, hızını sıfırla
        if (timeRewind != null && timeRewind.IsRewinding())
        {
            rb.linearVelocity = Vector2.zero;
        }
        else 
        {
            // Geri sarma modunda değilsek, normal hızımızla
            // kendi 'yukarı' yönümüze (baktığımız yöne) git
            rb.linearVelocity = transform.up * speed;
        }
    }

    // "Is Trigger" olan collider'ımıza bir şey çarptığında
    void OnTriggerEnter2D(Collider2D other)
    {
        // Eğer zaman geri sarılıyorsa çarpışmayı sayma
        if (timeRewind != null && timeRewind.IsRewinding())
        {
            return;
        }

        // --- YENİ ÇÖZÜM KODU ---
        // Eğer çarptığımız şey de bir TETİKLEYİCİ ise (düşmanın görüş alanı gibi),
        // bunu "vuruş" olarak sayma ve fonksiyonu terk et.
        if (other.isTrigger)
        {
            return; // Bu bir vuruş değil, muhtemelen bir görüş alanı.
        }
        // --- ÇÖZÜM KODU BİTTİ ---

        // Düşmana çarptıysak (ve bu bir tetikleyici değilse, yani 'bedeni' ise)
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Düşmanı yok et
            Destroy(gameObject); // Kendini (mermiyi) yok et
        }
    }
}