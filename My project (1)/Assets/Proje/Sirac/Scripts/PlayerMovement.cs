using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class PlayerMovement : MonoBehaviour
{
    [Header("Hareket & Savaş")]
    public float moveSpeed = 5f; 

    [Header("Can Ayarları")]
    public int maxHealth = 100; 
    public int currentHealth;   
    //public Slider healthBar;    

    [Header("Cheat (Ulti) Ayarları")]
    
    //public Slider cheatBar;      
    public int maxCheat = 100;   
    public int currentCheat = 0; 

    // --- YENİ EKLENEN KISIM: KAMERA TİTREME AYARLARI ---
    [Header("Kamera Titreme Ayarları (Camera Shake)")]
    public float shootShakeDuration = 0.1f; // Ateş edince ne kadar sürsün?
    public float shootShakePower = 0.1f;    // Ateş edince ne kadar şiddetli olsun?
    
    public float damageShakeDuration = 0.2f; // Hasar alınca ne kadar sürsün?
    public float damageShakePower = 0.3f;    // Hasar alınca ne kadar şiddetli olsun?

    public static PlayerMovement Instance;

    // ---------------------------------------------------

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
    }

    void Shoot()
    {
        // TİTREME BURADA ÇAĞRILIYOR (Senin ayarladığın değerlerle)
        if (CameraShake.instance != null)
        {
            CameraShake.instance.Shake(shootShakeDuration, shootShakePower); 
        }
    }

    public void AddCheatCharge(int amount)
    {
        currentCheat += amount;
        if (currentCheat > maxCheat) currentCheat = maxCheat;
    }

    public bool TryUseCheat()
    {
        if (currentCheat >= maxCheat)
        {
            currentCheat = 0; 
            return true; 
        }
        return false; 
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || (timeRewind != null && timeRewind.IsRewinding())) return;
        if (Time.time - lastDamageTime < damageCooldown) return;

        currentHealth -= damage;
        lastDamageTime = Time.time; 
        Debug.Log("Hasar alındı! Kalan Can: " + currentHealth);
        

        // TİTREME BURADA ÇAĞRILIYOR (Senin ayarladığın değerlerle)
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