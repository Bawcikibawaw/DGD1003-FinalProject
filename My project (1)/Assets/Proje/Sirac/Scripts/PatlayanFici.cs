using UnityEngine;

public class PatlayanFici : MonoBehaviour
{
    [Header("Fıçı Ayarları")]
    public int health = 30;
    public float patlamaYaricapi = 3f;
    public int patlamaHasari = 50;
    
    [Header("Efektler")]
    public GameObject patlamaEfekti; 
    public AudioClip patlamaSesi;    

    // --- KRİTİK KORUMA DEĞİŞKENİ ---
    private bool patladi = false; 

    public void TakeDamage(int damage)
    {
        // EĞER ZATEN PATLADIYSA HİÇBİR ŞEY YAPMA! (Çökme Engelleyici)
        if (patladi) return;

        health -= damage;
        if (health <= 0)
        {
            Patla();
        }
    }

    void Patla()
    {
        // Kilidi kapat, bir daha kimse bu fıçıya hasar veremez
        patladi = true;

        // 1. Etraftakileri Bul
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, patlamaYaricapi);
        
        foreach (Collider2D hitObj in hitColliders)
        {
            // A) Düşmana hasar ver
            Enemy enemy = hitObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(patlamaHasari, false); // Fıçı kritiği olmaz (false)
            }
            
            // B) Zincirleme Patlama (Diğer fıçılar)
            PatlayanFici fici = hitObj.GetComponent<PatlayanFici>();
            if (fici != null && fici != this)
            {
                // Diğer fıçıya hasar ver (O da kendi kontrolünü yapacak)
                fici.TakeDamage(patlamaHasari);
            }
        }

        // 2. Görsel Efekt
        if (patlamaEfekti != null)
        {
            Instantiate(patlamaEfekti, transform.position, Quaternion.identity);
        }

        // 3. Ses
        if (patlamaSesi != null)
        {
            AudioSource.PlayClipAtPoint(patlamaSesi, transform.position);
        }

        // 4. Yok Et
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, patlamaYaricapi);
    }
}