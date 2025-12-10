using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage = 20; // Oyuncuya kaç hasar versin?

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. OYUNCUYA ÇARPARSA
        if (collision.gameObject.CompareTag("Player"))
        {
            // Oyuncunun scriptine ulaş
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            
            // Eğer script varsa ve ölümsüz değilse hasar ver
            if (player != null)
            {
                // Geri itme efekti (Fiziksel vuruş)
                Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    Vector2 pushDir = (collision.transform.position - transform.position).normalized;
                    playerRb.AddForce(pushDir * 500f); // Geri fırlat
                }

                player.TakeDamage(damage);
            }
        }

        // 2. DÜŞMANA ÇARPARSA
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Düşmanı direkt yok et (Kıyma makinesi gibi)
            Destroy(collision.gameObject);
        }
    }
}