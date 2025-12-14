using UnityEngine;

public class PatlayanFici : MonoBehaviour
{
    [Header("Fıçı Ayarları")]
    public int health = 30;         
    public float patlamaAlani = 3f; 
    public int patlamaHasari = 50; 
    public float itmeGucu = 10f;    

    [Header("Efektler")]
    public GameObject patlamaEfekti; 

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Patla();
        }
    }

    void Patla()
    {
        // Etraftaki herkesi (Düşman, Kutu vs) bul
        Collider2D[] vurulanlar = Physics2D.OverlapCircleAll(transform.position, patlamaAlani);

        foreach (Collider2D nesne in vurulanlar)
        {
            // Düşmana hasar ver
            Enemy enemy = nesne.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(patlamaHasari);
            }

            // Fiziksel objeleri fırlat
            Rigidbody2D rb = nesne.GetComponent<Rigidbody2D>();
            if (rb != null && rb != GetComponent<Rigidbody2D>())
            {
                Vector2 direction = nesne.transform.position - transform.position;
                rb.AddForce(direction.normalized * itmeGucu, ForceMode2D.Impulse);
            }
        }

        // Efekt varsa oluştur
        if (patlamaEfekti != null)
        {
            Instantiate(patlamaEfekti, transform.position, Quaternion.identity);
        }

        // Fıçıyı yok et
        Destroy(gameObject);
    }
    
    // Editörde alanı görmek için kırmızı çizgi
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patlamaAlani);
    }
}