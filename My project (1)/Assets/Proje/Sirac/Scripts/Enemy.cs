using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Can Ayarları")]
    // Not: Bu değişkenleri Start içinde rastgele belirleyeceğiz
    private int currentHealth;

    [Header("UI & Görsel")]
    public Slider healthBar; 
    private SpriteRenderer sr;
    private Color originalColor; // Düşmanın orijinal rengini (Yeşil/Kırmızı/Beyaz) saklar

    [Header("Efektler")]
    public GameObject damagePopupPrefab; // Uçan yazı
    public AudioClip hitSound; // Vurulma Sesi

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        EnemyMovement movement = GetComponent<EnemyMovement>(); // Hızını değiştirmek için

        // --- RASTGELE DÜŞMAN TİPİ BELİRLEME ---
        int zar = Random.Range(0, 100); // 0 ile 100 arası zar at

        if (zar < 30) 
        {
            // --- TİP 1: KOŞUCU (%30 Şans) ---
            // Küçük, Çok Hızlı, Az Canlı, Parlak Yeşil
            transform.localScale = new Vector3(0.8f, 0.8f, 1f); 
            if (movement != null) movement.moveSpeed = 5f; 
            currentHealth = 60; 
            if (sr != null) sr.color = new Color(0.5f, 1f, 0.5f); 
        }
        else if (zar > 85) 
        {
            // --- TİP 2: TANK (%15 Şans) ---
            // Dev, Çok Yavaş, Çok Canlı, Koyu Kırmızı
            transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            if (movement != null) movement.moveSpeed = 1.5f; 
            currentHealth = 400; 
            if (sr != null) sr.color = new Color(1f, 0.5f, 0.5f); 
        }
        else 
        {
            // --- TİP 3: NORMAL (%55 Şans) ---
            // Standart boyut ve hız, Beyaz
            transform.localScale = Vector3.one; 
            if (movement != null) movement.moveSpeed = 3f; 
            currentHealth = 150; 
            if (sr != null) sr.color = Color.white; 
        }

        // Seçilen rengi hafızaya al (Flash bitince buna dönecek)
        if (sr != null) originalColor = sr.color;

        // Can Barını Ayarla
        if (healthBar != null)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = currentHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(int damage, bool isCritical)
    {
        currentHealth -= damage;

        if (healthBar != null) healthBar.value = currentHealth;
        
        // Vurulma efektini başlat
        if (sr != null) StartCoroutine(FlashEffect());

        // 1. HASAR YAZISI ÇIKAR
        if (damagePopupPrefab != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(damage, isCritical);
        }

        // 2. SES ÇAL
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        if (currentHealth <= 0) Die();
    }

    IEnumerator FlashEffect()
    {
        // Vurulunca belirgin bir KIRMIZI olsun (Beyaz düşmanlarda da belli olsun diye)
        sr.color = new Color(1f, 0.2f, 0.2f); 
        
        yield return new WaitForSeconds(0.1f); // 0.1 saniye bekle
        
        // Sonra orijinal rengine (Yeşil, Tank Kırmızısı veya Beyaz) dönsün
        sr.color = originalColor; 
    }

    void Die()
    {
        Destroy(gameObject);
    }
}