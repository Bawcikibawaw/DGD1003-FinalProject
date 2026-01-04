using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;
    public float lifeTime = 3f; // 3 saniye sonra kendi kendini yok etsin

    private Vector3 targetPosition;

    void Start()
    {
        // Player'ı bul
        if (PlayerMovement.Instance != null)
        {
            targetPosition = PlayerMovement.Instance.transform.position;
            
            // Player'a doğru dön (Rotasyon hesabı)
            Vector3 direction = targetPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // --- DÜZELTME BURADA ---
            // Eğer resmin YUKARI bakıyorsa -90 çıkar.
            // Eğer resmin SOLA bakıyorsa 180 ekle.
            // Genelde oklar yukarı çizildiği için -90 sorunu çözer.
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        
        // Mermi sonsuza kadar gitmesin, süre dolunca yok olsun
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Kendi "YUKARISINA" doğru git (Çünkü resmi -90 çevirince YUKARI yönü ileri olmuş oldu)
        // EĞER -90 YAPTIYSAK ARTIK 'Right' YERİNE 'Up' KULLANMALIYIZ Kİ UCU ÖNDE GİTSİN
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Eğer Oyuncuya çarparsa
        if (hitInfo.CompareTag("Player"))
        {
            PlayerMovement player = hitInfo.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damage); // Hasar ver
            }
            Destroy(gameObject); // Mermiyi yok et
        }
        // Duvara çarparsa (Tag'i 'Wall' veya 'Obstacle' olanlar)
        else if (hitInfo.CompareTag("Wall") || hitInfo.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}