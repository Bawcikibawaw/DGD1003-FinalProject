using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement; 

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public GameObject bulletPrefab; 
    public Transform firePoint;     

    // --- YENİ DEĞİŞKEN ---
    // Public yaptık ki başka scriptler veya Unity Editörü bunu değiştirebilsin
    public bool isInvincible = false; 

    private Rigidbody2D rb;
    private Vector2 moveInput;   
    private Vector2 mousePos;    
    private Camera cam; 
    private TimeRewind timeRewind; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main; 
        timeRewind = GetComponent<TimeRewind>(); 
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
            
            // "G" TUŞU KODUNU BURADAN SİLDİK
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (timeRewind != null && timeRewind.IsRewinding()) return;

        // --- KONTROL ---
        // Eğer bu karakter "isInvincible" (Ölümsüz) olarak işaretlendiyse ölme!
        if (isInvincible)
        {
            return; 
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("OYUNCU YAKALANDI! Yeniden başlatılıyor...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}