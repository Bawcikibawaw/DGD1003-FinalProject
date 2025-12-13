using UnityEngine;
using UnityEngine.UI; // Can barı (Slider) için gerekli
using System.Collections; // Coroutine (Yanıp sönme) için gerekli

public class Enemy : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int minHealth = 50;  // En az can
    public int maxHealth = 150; // En çok can
    
    // Anlık canı Inspector'da görmene gerek yok, oyun başlayınca belirlenir.
    private int currentHealth; 

    [Header("UI & Görsel")]
    public Slider healthBar; // Unity'de oluşturduğun Slider'ı buraya sürükle
    
    private SpriteRenderer sr;
    private Color originalColor;
    
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
        // Düşmanın orijinal rengini kaydet (Yanıp sönünce geri dönmek için)
        if (sr != null) originalColor = sr.color;

        // --- 1. RASTGELE CAN BELİRLEME ---
        currentHealth = Random.Range(minHealth, maxHealth);

        // --- 2. CAN BARINI AYARLAMA ---
        if (healthBar != null)
        {
            healthBar.maxValue = currentHealth; // Barın maksimumu, tutulan can olsun
            healthBar.value = currentHealth;    // Barı fulle
        }
        
        // --- 3. BOYUTU CANA GÖRE AYARLAMA (BONUS) ---
        // Canı 100 olan 1 boyutta, 150 olan 1.5 boyutta olur.
        float sizeScale = currentHealth / 100f;
        // Çok küçük olmasın diye en az 0.8 ile sınırla
        sizeScale = Mathf.Max(sizeScale, 0.8f); 
        transform.localScale = new Vector3(sizeScale, sizeScale, 1);
    }

    public void TakeDamage(int damage)
    {
        // --- HASAR HESAPLAMA ---
        currentHealth -= damage;
        
        // KONSOLA YAZDIR (Sorun olursa buradan anlarız)
        Debug.Log("Düşman vuruldu! Gelen Hasar: " + damage + " | Kalan Can: " + currentHealth);

        // --- CAN BARINI GÜNCELLE ---
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // --- PARLAMA EFEKTİ (HIT FLASH) ---
        if (sr != null) StartCoroutine(FlashEffect());

        // --- ÖLÜM KONTROLÜ ---
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Düşmanı 0.1 saniyeliğine BEYAZ yapar, sonra eski rengine döndürür
    IEnumerator FlashEffect()
    {
        sr.color = Color.white; 
        yield return new WaitForSeconds(0.1f); 
        sr.color = originalColor; 
    }

    void Die()
    {
        // İleride buraya patlama efekti veya ses ekleyebilirsin
        // Şimdilik sadece yok ediyoruz.
        Destroy(gameObject);
    }
}