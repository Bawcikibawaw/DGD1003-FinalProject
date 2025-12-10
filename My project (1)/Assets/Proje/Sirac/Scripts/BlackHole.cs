using UnityEngine;

public class BlackHole : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. DÜŞMAN GİRERSE
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); // Direkt yok et
        }

        // 2. OYUNCU GİRERSE
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                // 9999 hasar vererek kesin öldür (God mode açıksa ölmez, bu da havalı olur)
                player.TakeDamage(9999); 
            }
        }
    }
}