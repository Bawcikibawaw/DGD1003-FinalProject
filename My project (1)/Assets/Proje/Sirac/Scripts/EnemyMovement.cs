using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Enemy.cs'in erişebilmesi için 'public' yaptık
    public float moveSpeed = 3f; 
    
    private Transform player;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Oyuncuyu bul
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // --- GERİ TEPME (KNOCKBACK) KONTROLÜ ---
        // Eğer mermi yiyip uçuyorsa (hızı çok yüksekse), yürüme kodu karışmasın.
        if (rb.linearVelocity.magnitude > moveSpeed * 1.5f)
        {
            return;
        }
        
        // Normal Yürüme (Oyuncuya yönel)
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
}