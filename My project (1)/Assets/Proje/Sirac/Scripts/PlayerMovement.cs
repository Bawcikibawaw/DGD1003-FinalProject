using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement; 
using UnityEngine.UI; 

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public GameObject bulletPrefab; 
    public Transform firePoint;     

    [Header("Can Ayarları")]
    public int maxHealth = 100; 
    public int currentHealth;   
    public Slider healthBar;    

    [Header("Cheat (Ulti) Ayarları")]
    public Slider cheatBar;      // Mavi barı buraya sürükle
    public int maxCheat = 100;   // Bar kaça gelince dolsun?
    public int currentCheat = 0; // Şu anki doluluk

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
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // --- YENİ: CHEAT BAR BAŞLANGIÇ ---
        if (cheatBar != null)
        {
            cheatBar.maxValue = maxCheat;
            cheatBar.value = 0; // Boş başla
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

    // --- YENİ: ENERJİ EKLEME FONKSİYONU ---
    // Bunu mermi (Bullet) çağıracak
    public void AddCheatCharge(int amount)
    {
        currentCheat += amount;
        if (currentCheat > maxCheat) currentCheat = maxCheat;

        if (cheatBar != null) cheatBar.value = currentCheat;
    }

    // --- YENİ: ENERJİ HARCAMA KONTROLÜ ---
    // Bunu skiller (Ghost, GodMode vb.) çağıracak
    public bool TryUseCheat()
    {
        // Bar dolu mu?
        if (currentCheat >= maxCheat)
        {
            currentCheat = 0; // Barı boşalt
            if (cheatBar != null) cheatBar.value = currentCheat;
            return true; // "Evet, kullanabilirsin" de
        }
        else
        {
            Debug.Log("Bar henüz dolmadı!");
            return false; // "Hayır, kullanamazsın" de
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || (timeRewind != null && timeRewind.IsRewinding())) return;
        if (Time.time - lastDamageTime < damageCooldown) return;

        currentHealth -= damage;
        lastDamageTime = Time.time; 

        if (healthBar != null) healthBar.value = currentHealth;

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
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
}