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
    private SpriteRenderer sr;
    private Animator anim;

    private float lastDamageTime;
    private float damageCooldown = 1f; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; 
        timeRewind = GetComponent<TimeRewind>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        
        
        if (Instance != null && Instance != this) { Destroy(this); return; }
        Instance = this;
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            Vector2 move = Vector2.zero;
            if (Keyboard.current.wKey.isPressed){ move.y += 1;}
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
        
        float horizontalSpeed = Mathf.Abs(moveInput.x);
        anim.SetFloat("Speed", horizontalSpeed);
        
        Debug.Log("Animatöre giden hız: " + horizontalSpeed);
    }

    void FixedUpdate()
    {
        if (timeRewind != null && timeRewind.IsRewinding())
        {
            rb.linearVelocity = Vector2.zero; 
            return; 
        }
        
        // Hareket
        rb.linearVelocity = moveInput * moveSpeed;
        
        HandleFlipping();
    }

    void Shoot()
    {
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
    
    public void TakeDamage(int damage)
    {
        if (isInvincible || (timeRewind != null && timeRewind.IsRewinding())) return;
        if (Time.time - lastDamageTime < damageCooldown) return;

        currentHealth -= damage;
        lastDamageTime = Time.time; 
        

        Debug.Log("Health: " + currentHealth);
        
        if (CameraShake.instance != null)
        {
            CameraShake.instance.Shake(damageShakeDuration, damageShakePower); 
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- GÜNCELLENEN KISIM: ARTIK GAMEOVER MANAGER ÇAĞIRIYOR ---
    void Die()
    {
        Debug.Log("PLAYER DIED!");
        
        // 1. Game Over Ekranını Çağır
        if (GameOverManager.instance != null)
        {
            // Şimdilik 0 yolluyoruz, WaveManager ile bağlayınca oradan gerçek sayıyı alırız.
            GameOverManager.instance.TriggerGameOver(0); 
        }
        else
        {
            // Eğer sahnede Game Manager yoksa güvenlik için sahneyi yeniden yükle
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // 2. Oyuncuyu yok etme, sadece gizle (Kamera ve Manager bozulmasın)
        gameObject.SetActive(false);
    }
    
    private void HandleFlipping()
    {
        // Only attempt to flip if the SpriteRenderer exists and there is horizontal input
        if (sr != null && moveInput.x != 0)
        {
            // If moving right (positive input), ensure the sprite is NOT flipped (false)
            if (moveInput.x > 0)
            {
                sr.flipX = false;
            }
            // If moving left (negative input), flip the sprite (true)
            else if (moveInput.x < 0)
            {
                sr.flipX = true;
            }
        }
    }
}