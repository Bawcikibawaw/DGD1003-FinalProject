using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Can Ayarları")]
    private int currentHealth;

    [Header("Saldırı Ayarları (YENİ)")]
    public int attackDamage = 10;       // Oyuncuya kaç vursun?
    public float attackCooldown = 1.5f; // Kaç saniyede bir vursun?
    private float lastAttackTime;       // Son vuruş zamanı

    [Header("Loot & XP")]
    public GameObject lootPrefab; 
    [Range(0, 100)] public int dropChance = 20; 
    public float xpAmount = 20f; 
    public float cheatValueOnDeath = 10f; 

    [Header("UI & Görsel")]
    public Slider healthBar; 
    private SpriteRenderer sr;
    private Color originalColor; 

    [Header("Efektler")]
    public GameObject damagePopupPrefab; 
    public AudioClip hitSound; 

    // Bileşenler
    private Animator anim;
    private Rigidbody2D rb;
    private EnemyMovement movementScript; // Hareketi durdurmak için
    private bool isDead = false; 

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movementScript = GetComponent<EnemyMovement>(); 

        // --- RASTGELE DÜŞMAN TİPİ ---
        // (Buradaki kodların aynen kalıyor, sadece scale değerlerini büyüttük)
        int zar = Random.Range(0, 100); 

        if (zar < 30) // KOŞUCU
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 1f); 
            if (movementScript != null) movementScript.moveSpeed = 5f; 
            currentHealth = 60; 
            if (sr != null) sr.color = new Color(0.5f, 1f, 0.5f); 
            xpAmount = 15f; 
        }
        else if (zar > 85) // TANK
        {
            transform.localScale = new Vector3(4.5f, 4.5f, 1f);
            if (movementScript != null) movementScript.moveSpeed = 1.5f; 
            currentHealth = 400; 
            if (sr != null) sr.color = new Color(1f, 0.5f, 0.5f); 
            xpAmount = 50f; 
        }
        else // NORMAL
        {
            transform.localScale = new Vector3(3f, 3f, 1f); 
            if (movementScript != null) movementScript.moveSpeed = 3f; 
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

    void Update()
    {
        if (isDead) return;

        // Hareket animasyonu (Speed)
        if (anim != null && rb != null)
        {
            float speed = rb.linearVelocity.magnitude;
            anim.SetFloat("Speed", speed);
        }
    }

    // --- YENİ: ÇARPIŞMA VE SALDIRI MANTIĞI ---
    void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        // Çarptığım şey Oyuncu mu?
        if (collision.gameObject.CompareTag("Player"))
        {
            // Saldırı zamanı geldi mi?
            if (Time.time > lastAttackTime + attackCooldown)
            {
                Attack(collision.gameObject);
            }
        }
    }

    void Attack(GameObject playerObj)
    {
        lastAttackTime = Time.time;

        // 1. Animasyonu Oynat
        if (anim != null) anim.SetTrigger("Attack");

        // 2. Oyuncuya Hasar Ver (PlayerMovement scriptindeki TakeDamage'i çağır)
        PlayerMovement playerScript = playerObj.GetComponent<PlayerMovement>();
        if (playerScript != null)
        {
            playerScript.TakeDamage(attackDamage);
        }

        // 3. Saldırı sırasında düşmanı kısa süre dondur (Daha iyi hissettirir)
        StartCoroutine(StopMovementBriefly());
    }

    IEnumerator StopMovementBriefly()
    {
        if (movementScript != null) movementScript.enabled = false; // Hareketi kapat
        rb.linearVelocity = Vector2.zero; // Anında dur
        
        yield return new WaitForSeconds(0.5f); // 0.5 saniye bekle (Animasyon süresi kadar)
        
        if (!isDead && movementScript != null) movementScript.enabled = true; // Hareketi aç
    }
    // ------------------------------------------

    public void TakeDamage(int damage, bool isCritical)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (healthBar != null) healthBar.value = currentHealth;
        if (sr != null) StartCoroutine(FlashEffect());
        if (anim != null) anim.SetTrigger("Hurt");

        if (damagePopupPrefab != null)
        {
            Vector3 spawnPosition = transform.position + new Vector3(0, 0.5f, 0);
            GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(damage, isCritical);
        }

        if (hitSound != null) AudioSource.PlayClipAtPoint(hitSound, transform.position);

        if (currentHealth <= 0) Die();
    }

    public void TakeDamage(int damage) { TakeDamage(damage, false); }
    public void TakeDamage(int damage, float knockback) { TakeDamage(damage, false); }

    IEnumerator FlashEffect()
    {
        sr.color = new Color(1f, 0.2f, 0.2f); 
        yield return new WaitForSeconds(0.1f); 
        sr.color = originalColor; 
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (rb != null) rb.linearVelocity = Vector2.zero;
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        if (movementScript != null) movementScript.enabled = false; // Hareketi tamamen kapat

        if (anim != null) anim.SetTrigger("Die");

        if (LevelSystem.instance != null)
        {
            LevelSystem.instance.AddExperience(xpAmount);
            LevelSystem.instance.AddCheatValue(cheatValueOnDeath);
        }

        if (lootPrefab != null && Random.Range(0, 100) <= dropChance)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }

        if (PlayerMovement.Instance != null) PlayerMovement.Instance.AddCheatCharge(5);

        Destroy(gameObject, 1f); 
    }
}