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

    [Header("Level XP & Güç Sistemi")]
    public float xpAmount = 20f; // Bu düşman ölünce kaç XP versin?
    public float cheatValueOnDeath = 10f; // (YENİ) Ölünce Cheat Bar'a kaç değer eklesin?

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
            if (sr != null) sr.color = new Color(0.5f, 1f, 0.5f); 
            xpAmount = 15f; 
        }
        else if (zar > 85) // TANK (%15)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1f);
            if (movement != null) movement.moveSpeed = 1.5f; 
            currentHealth = 400; 
            if (sr != null) sr.color = new Color(1f, 0.5f, 0.5f); 
            xpAmount = 50f; 
        }
        else // NORMAL (%55)
        {
            transform.localScale = Vector3.one; 
            if (movement != null) movement.moveSpeed = 3f; 
            currentHealth = 150; 
            if (sr != null) sr.color = Color.white; 
            xpAmount = 25f;
        }

        if (sr != null) originalColor = sr.color;

        if (healthBar != null)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = currentHealth;
            healthBar.value = currentHealth;
        }
    }

    // --- OVERLOAD 1: ANA HASAR ALMA FONKSİYONU ---
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

    // --- OVERLOAD 2: Tek parametre (int damage) gönderilirse (Mermi, Patlayan Fıçı) ---
    public void TakeDamage(int damage)
    {
        // Varsayılan olarak kritik değil (false) kabul et
        TakeDamage(damage, false);
    }
    
    // --- OVERLOAD 3: Üç parametre (int damage, float knockback) gönderilirse ---
    public void TakeDamage(int damage, float knockback)
    {
        // Knockback'i şimdilik yok sayıp, ana fonksiyona yolla
        TakeDamage(damage, false);
    }

    IEnumerator FlashEffect()
    {
        sr.color = new Color(1f, 0.2f, 0.2f); 
        yield return new WaitForSeconds(0.1f); 
        sr.color = originalColor; 
    }

    void Die()
    {
        // 1. XP VERME İŞLEMİ 
        if (LevelSystem.instance != null)
        {
            LevelSystem.instance.AddExperience(xpAmount);
        }
        
        // 2. CHEAT BAR DOLDURMA İŞLEMİ (YENİ EKLENDİ!)
        // Eğer Cheat Bar sistemi (PlayerStats veya LevelSystem içinde) varsa
        // Bu kod LevelSystem içinde AddCheatValue fonksiyonunun olduğunu varsayar.
        if (LevelSystem.instance != null)
        {
            // LevelSystem.cs scriptinin içinde AddCheatValue(float amount) fonksiyonu OLMALI!
            LevelSystem.instance.AddCheatValue(cheatValueOnDeath);
        }

        // 3. LOOT DÜŞÜRME İŞLEMİ
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