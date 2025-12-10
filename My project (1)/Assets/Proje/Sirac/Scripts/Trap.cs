using System.Collections;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public int damage = 20; // Oyuncuya kaç hasar versin?
    private Rigidbody2D playerRigidbody;
    private HingeJoint2D trapHinge;

    void Awake()
    {
        trapHinge = GetComponent<HingeJoint2D>();
        
        //StartCoroutine("FindAndCachePlayerRigidbody");
    }

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
    
    private IEnumerator FindAndCachePlayerRigidbody()
    {
        yield return null;
        
        // 1. Tag ile objeyi bul
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            // 2. Objenin Rigidbody2D bileşenini al ve sınıf değişkenine ata
            playerRigidbody = playerObject.GetComponent<Rigidbody2D>();

            if (playerRigidbody != null)
            {
                Debug.Log("Player" + " objesi bulundu ve Rigidbody'si değişkene atandı.");
            }
            else
            {
                Debug.LogError("Player" + " objesi bulundu ancak üzerinde Rigidbody2D bileşeni eksik!");
            }
        }
        else
        {
            Debug.LogWarning("Player" + " etiketli obje sahnede bulunamadı!");
        }
        
        if (trapHinge != null && playerRigidbody != null)
        {
            //  ANA İŞLEM: Player Rigidbody'yi tuzağın Hinge Joint'ine bağla
            trapHinge.connectedBody = playerRigidbody;
            
            Debug.Log("Önbelleğe alınan Player (" + playerRigidbody.gameObject.name + ") başarıyla tuzağa bağlandı.");
        }
        else
        {
            Debug.LogError("Bağlanma başarısız! Hinge Joint veya Player Rigidbody referansı eksik. Player'ı tekrar bulmaya çalışın.");
            // Eğer bağlantı sırasında hala null ise, belki FindAndCachePlayerRigidbody() çağrısını bir kez daha denemek gerekebilir.
        }
    }
    
}