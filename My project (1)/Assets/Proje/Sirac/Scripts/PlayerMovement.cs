using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement; 
using UnityEngine.UI; // <-- YENİ: UI (Can Barı) kullanmak için şart!

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public GameObject bulletPrefab; 
    public Transform firePoint;     

    // --- YENİ CAN DEĞİŞKENLERİ ---
    public int maxHealth = 100; // Maksimum can
    public int currentHealth;   // O anki can
    public Slider healthBar;    // Ekrandaki Slider'ı buraya sürükleyeceğiz

    public bool isInvincible = false; // God Mode için

    private Rigidbody2D rb;
    private Vector2 moveInput;   
    private Vector2 mousePos;    
    private Camera cam; 
    private TimeRewind timeRewind; 

    // Hasar alma sıklığını kontrol etmek için (Cooldown)
    private float lastDamageTime;
    private float damageCooldown = 1f; // 1 saniyede bir hasar alabilsin

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; 
        timeRewind = GetComponent<TimeRewind>(); 

        // Oyuna başlarken canı fulle
        currentHealth = maxHealth;
        
        // Eğer healthBar kutusunu doldurduysak ayarlarını yap
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
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
        
        if (Mouse.current != null) 
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
        
        rb.linearVelocity = moveInput * moveSpeed;

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; 
        rb.MoveRotation(angle);
    }

    void Shoot()
    {
        Transform spawnPoint = (firePoint != null) ? firePoint : transform;
        
        Vector2 lookDir = mousePos - (Vector2)spawnPoint.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

        Instantiate(bulletPrefab, spawnPoint.position, bulletRotation); 
    }

    // --- CAN AZALTMA FONKSİYONU ---
    public void TakeDamage(int damage)
    {
        // God Mode açıksa veya geri sarıyorsak hasar alma
        if (isInvincible || (timeRewind != null && timeRewind.IsRewinding())) return;

        // Son hasardan bu yana 1 saniye geçmediyse hasar alma (Makinalı tüfek gibi can gitmesin)
        if (Time.time - lastDamageTime < damageCooldown) return;

        // Canı azalt
        currentHealth -= damage;
        lastDamageTime = Time.time; // Son hasar zamanını güncelle
        Debug.Log("Hasar alındı! Kalan Can: " + currentHealth);

        // Can barını güncelle
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        // Öldük mü?
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("OYUNCU ÖLDÜ! Yeniden başlatılıyor...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Çarpışma olduğunda
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Direkt ölmek yerine 20 hasar al
            TakeDamage(20); 
        }
    }
    
    // Temas devam ettiği sürece (Enemy içinde kalınca)
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Süre dolduysa tekrar hasar al
            TakeDamage(20);
        }
    }
}