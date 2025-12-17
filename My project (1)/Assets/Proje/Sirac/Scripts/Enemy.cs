using UnityEngine;
using UnityEngine.UI; 
using System.Collections;
using System.ComponentModel.Design.Serialization;

public class Enemy : MonoBehaviour
{
    [Header("Can Ayarları")]
    public int minHealth = 50;
    public int maxHealth = 150;
    private int currentHealth;

    [Header("UI & Görsel")]
    public Slider healthBar; 
    private SpriteRenderer sr;
    private Color originalColor;

    [Header("Efektler")]
    public GameObject damagePopupPrefab; // Sarı/Kırmızı yazı prefabı

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;

        // Rastgele Canla Başla
        currentHealth = Random.Range(minHealth, maxHealth);

        // Can Barını Ayarla
        if (healthBar != null)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = currentHealth;
            healthBar.value = currentHealth;
        }
    }

    // ARTIK 2 PARAMETRE ALIYOR: (Hasar, Kritik mi?)
    public void TakeDamage(int damage, bool isCritical)
    {
        currentHealth -= damage;

        // 1. Can Barını Güncelle
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // 2. Beyaz Yanıp Sönme
        if (sr != null)
        {
            StartCoroutine(FlashEffect());
        }

        // 3. HASAR YAZISINI OLUŞTUR
        if (damagePopupPrefab != null)
        {
            // Yazıyı kafasının üstünde oluştur
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);
            
            // Yazı scriptini bul
            DamagePopup popupScript = popup.GetComponent<DamagePopup>();
            if (popupScript != null)
            {
                // Yazıya hem hasarı hem de kritik bilgisini gönder
                // (DamagePopup.cs dosyanın güncel olması lazım!)
                popupScript.Setup(damage, isCritical);
            }
        }

        // 4. Ölüm Kontrolü
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashEffect()
    {
        sr.color = Color.white; 
        yield return new WaitForSeconds(0.1f); 
        sr.color = originalColor; 
    }

    void Die()
    {
        Destroy(gameObject);
        PlayerMovement.Instance.AddCheatCharge(5);
    }
}