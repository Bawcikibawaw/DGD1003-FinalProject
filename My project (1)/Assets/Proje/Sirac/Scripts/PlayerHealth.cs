using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Oyunu yeniden başlatmak için gerekli

public class PlayerHealth : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Bağlantısı")]
    public Slider healthSlider; // Ekranda can barı varsa buraya sürükle

    void Start()
    {
        currentHealth = maxHealth;
        
        // Can barı varsa başlangıç değerini ayarla
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    // Düşmanlar sana vurunca bu çalışacak
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // UI Güncelle
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        // Ölüm kontrolü
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- İŞTE HATA VEREN KISIM İÇİN GEREKLİ FONKSİYON ---
    // Yerden Can Kutusu alınca bu çalışacak
    public void Heal(int amount)
    {
        currentHealth += amount;

        // Can asla 100'ü (max) geçmesin
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // UI Güncelle
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        
        Debug.Log("Can Dolduruldu! Şu anki Can: " + currentHealth);
    }

    void Die()
    {
        // Oyuncu ölünce sahne yeniden başlasın
        Debug.Log("ÖLDÜN!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}