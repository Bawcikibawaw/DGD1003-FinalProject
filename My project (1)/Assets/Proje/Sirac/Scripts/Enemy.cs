using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

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
    public GameObject damagePopupPrefab; // Sadece bunu bağlayacaksın (Sarı Yazı)

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;

        currentHealth = Random.Range(minHealth, maxHealth);

        if (healthBar != null)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = currentHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // 1. Can Barı Güncelle
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // 2. Yanıp Sönme
        if (sr != null)
        {
            StartCoroutine(FlashEffect());
        }

        // 3. UÇAN HASAR YAZISI (İstediğin özellik)
        if (damagePopupPrefab != null)
        {
            // Yazıyı kafasının üstünde oluştur
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);
            
            // Sayıyı yazdır
            DamagePopup popupScript = popup.GetComponent<DamagePopup>();
            if (popupScript != null)
            {
                popupScript.Setup(damage);
            }
        }

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
        // Hiçbir şey düşürmeden direkt yok ol
        Destroy(gameObject);
    }
}