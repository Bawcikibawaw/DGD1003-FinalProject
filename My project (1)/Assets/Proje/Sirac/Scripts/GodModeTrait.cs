using UnityEngine;

public class GodModeTrait : MonoBehaviour
{
    void Start()
    {
        // 1. Aynı obje üzerindeki PlayerMovement script'ine ulaş
        PlayerMovement movement = GetComponent<PlayerMovement>();

        if (movement != null)
        {
            // 2. Onu ölümsüz yap
            movement.isInvincible = true;
            Debug.Log("Özel Karakter Aktif: Ölümsüzlük Açıldı!");
        }

        // 3. Karakterin rengini Altın Sarısı yap (Farkı görelim diye)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.yellow;
        }
    }
}