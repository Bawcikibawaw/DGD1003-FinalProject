using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Can Ayarları")]
    private int currentHealth;

    [Header("Loot (Eşya) Sistemi")]
    public GameObject lootPrefab; // Düşecek eşya (HealthPickup)
    [Range(0, 100)] public int dropChance = 20; // %20 düşme şansı

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

        // --- RASTGELE DÜŞMAN TİPİ ---
        int zar = Random.Range(0, 100); 

        if (zar < 30) // KOŞUCU
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 1f); 
            if (movement != null) movement.moveSpeed = 5f; 
            currentHealth = 60; 
            if (sr != null) sr.color = new Color(0.5f, 1f, 0.5f); 
        }
        else if (zar > 85) // TANK
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            if (movement != null) movement.moveSpeed = 1.5f; 
            currentHealth = 400; 
            if (sr != null) sr.color = new Color(1f, 0.5f, 0.5f); 
        }
        else // NORMAL
        {
            transform.localScale = Vector3.one; 
            if (movement != null) movement.moveSpeed = 3f; 
            currentHealth = 150; 
            if (sr != null) sr.color = Color.white; 
        }

        if (sr != null) originalColor = sr.color;

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
        sr.color = new Color(1f, 0.2f, 0.2f); 
        yield return new WaitForSeconds(0.1f); 
        sr.color = originalColor; 
    }

    void Die()
    {
        // --- LOOT DÜŞÜRME MANTIĞI ---
        if (lootPrefab != null)
        {
            // 0 ile 100 arası sayı tut, eğer şans oranından küçükse eşya düşür
            if (Random.Range(0, 100) <= dropChance)
            {
                Instantiate(lootPrefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}