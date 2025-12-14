using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Mermi Ayarları")]
    public float speed = 20f;
    public int damage = 15;      
    public float lifetime = 3f;  

    [Header("Efektler")]
    public GameObject bloodPrefab; 

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * speed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 1. DÜŞMANA ÇARPARSA
        if (hitInfo.CompareTag("Enemy"))
        {
            // Kan Efekti
            if (bloodPrefab != null)
            {
                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                Instantiate(bloodPrefab, hitInfo.transform.position, randomRotation);
            }

            // Geri İtme (Knockback)
            Rigidbody2D enemyRb = hitInfo.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                enemyRb.AddForce(transform.up * 400f, ForceMode2D.Impulse);
            }

            // Hasar Ver
            Enemy enemyScript = hitInfo.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }
            
            Destroy(gameObject);
        }

        // 2. PATLAYAN FIÇIYA ÇARPARSA (İsim değiştiği için burayı güncelledik!)
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