using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Can Ayarları")]
    private int currentHealth;

    [Header("Loot (Eşya) Sistemi")]
    public GameObject lootPrefab; // Can kutusu prefabını buraya sürükle
    [Range(0, 100)] public int dropChance = 20; // %20 düşme şansı

    [Header("Level XP Sistemi")]
    public float xpAmount = 20f; // Bu düşman ölünce kaç XP versin?

    [Header("UI & Görsel")]
    public Slider healthBar; 
    private SpriteRenderer sr;
    private Color originalColor; 

    [Header("Efektler")]
    public GameObject damagePopupPrefab; 
    public AudioClip hitSound; 

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        EnemyMovement movement = GetComponent<EnemyMovement>(); 

        // --- RASTGELE DÜŞMAN TİPİ BELİRLEME ---
        int zar = Random.Range(0, 100); 

        if (zar < 30) // KOŞUCU (%30)
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 1f); 
            if (movement != null) movement.moveSpeed = 5f; 
            currentHealth = 60; 
            if (sr != null) sr.color = new Color(0.5f, 1f, 0.5f); // Açık Yeşil
            xpAmount = 15f; // Kolay öldüğü için az XP
        }
        else if (zar > 85) // TANK (%15)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            if (movement != null) movement.moveSpeed = 1.5f; 
            currentHealth = 400; 
            if (sr != null) sr.color = new Color(1f, 0.5f, 0.5f); // Açık Kırmızı
            xpAmount = 50f; // Zor öldüğü için çok XP
        }
        else // NORMAL (%55)
        {
            transform.localScale = Vector3.one; 
            if (movement != null) movement.moveSpeed = 3f; 
            currentHealth = 150; 
            if (sr != null) sr.color = Color.white; 
            xpAmount = 25f;
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
        
        if (sr != null) StartCoroutine(FlashEffect());

        // HASAR YAZISI
        if (damagePopupPrefab != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(damage, isCritical);
        }

        // SES
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
        }

        if (currentHealth <= 0) Die();
    }

    IEnumerator FlashEffect()
    {
        // Vurulunca KIPKIRMIZI olsun
        sr.color = new Color(1f, 0.2f, 0.2f); 
        yield return new WaitForSeconds(0.1f); 
        // Sonra kendi orijinal rengine dönsün
        sr.color = originalColor; 
    }

    void Die()
    {
        // 1. XP VERME İŞLEMİ (LevelSystem'e bağlanır)
        if (LevelSystem.instance != null)
        {
            LevelSystem.instance.AddExperience(xpAmount);
        }

        // 2. LOOT DÜŞÜRME İŞLEMİ
        if (lootPrefab != null)
        {
            if (Random.Range(0, 100) <= dropChance)
            {
                Instantiate(lootPrefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}