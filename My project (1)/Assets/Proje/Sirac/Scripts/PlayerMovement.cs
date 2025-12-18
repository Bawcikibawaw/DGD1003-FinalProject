using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class PlayerMovement : MonoBehaviour
{
    [Header("Hareket & Savaş")]
    public float moveSpeed = 5f; 

    // --- EKSİK OLAN KISIMLAR EKLENDİ ---
    [Header("Silah Ayarları")]
    public GameObject bulletPrefab; // Mermi prefabını buraya sürükle
    public Transform firePoint;     // Silahın ucundaki objeyi buraya sürükle
    // -----------------------------------

    [Header("Can Ayarları")]
    public int maxHealth = 100; 
    public int currentHealth;   
    
    [Header("Cheat (Ulti) Ayarları")]
    public int maxCheat = 100;   
    public int currentCheat = 0; 

    [Header("Kamera Titreme Ayarları")]
    public float shootShakeDuration = 0.1f;
    public float shootShakePower = 0.1f;
    public float damageShakeDuration = 0.2f; 
    public float damageShakePower = 0.3f;    

    public static PlayerMovement Instance;

    [Header("Durumlar")]
    public bool isInvincible = false; 

    private Rigidbody2D rb;
    private Vector2 moveInput;   
    private Vector2 mousePos;    
    private Camera cam; 
    private TimeRewind timeRewind; 

    private float lastDamageTime;
    private float damageCooldown = 1f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; 
        timeRewind = GetComponent<TimeRewind>();

        currentHealth = maxHealth;
        
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            Vector2 move = Vector2.zero;
            if (Keyboard.current.wKey.isPressed) move.y += 1;
            if (Keyboard.current.sKey.isPressed) move.y -= 1;
            if (Keyboard.current.aKey.isPressed) move.x -= 1;
            if (Keyboard.current.dKey.isPressed) move.x += 1;
            moveInput = move.normalized; 
        }
        
        if (Mouse.current != null && cam != null) 
        {
            mousePos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (timeRewind == null || !timeRewind.IsRewinding())
            {
                Shoot();
            }
        }
    }

    void FixedUpdate()
    {
        if (timeRewind != null && timeRewind.IsRewinding())
        {
            rb.linearVelocity = Vector2.zero; 
            return; 
        }
        
        // 1. HAREKET
        rb.linearVelocity = moveInput * moveSpeed;

        // 2. MOUSE'A DÖNME (ROTATION) - BU EKSİKTİ EKLENDİ
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void Shoot()
    {
        // 1. KAMERA TİTREMESİ
        if (CameraShake.instance != null)
        {
            CameraShake.instance.Shake(shootShakeDuration, shootShakePower); 
        }

        // 2. MERMİ OLUŞTURMA - BU EKSİKTİ EKLENDİ
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    public void AddCheatCharge(int amount)
    {
        currentCheat += amount;
        if (currentCheat > maxCheat) currentCheat = maxCheat;
    }
    
    public void TakeDamage(int damage)
    {
        if (isInvincible || (timeRewind != null && timeRewind.IsRewinding())) return;
        if (Time.time - lastDamageTime < damageCooldown) return;

        currentHealth -= damage;
        lastDamageTime = Time.time; 
        Debug.Log("Hasar alındı! Kalan Can: " + currentHealth);
        
        if (CameraShake.instance != null)
        {
            CameraShake.instance.Shake(damageShakeDuration, damageShakePower); 
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("OYUNCU ÖLDÜ!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (timeRewind != null && timeRewind.IsRewinding()) return;
        if (isInvincible) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20); 
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        if (timeRewind != null && timeRewind.IsRewinding()) return;
        if (isInvincible) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20);
        }
    }
}