using UnityEngine;
using System.Collections.Generic;

public class KnightWeapon : MonoBehaviour
{
    [Header("Combo Settings")]
    public List<GameObject> comboPrefabs; 
    public Transform firePoint;
    public float comboWindow = 1.0f; 
    public float baseFireRate = 0.2f; 

    [Header("Projectile Settings")]
    public float projectileSpeed = 15f;
    public float rotationOffset = -90f; // Adjust based on your sprite orientation

    private int currentComboIndex = 0;
    private float lastClickTime = 0f;
    private float nextFireTime = 0f;
    private Animator anim;
    private PlayerCheat cheatSystem; // Cached reference

    void Start()
    {
        anim = GetComponent<Animator>();
        // Find the cheat system on the player
        cheatSystem = GetComponentInParent<PlayerCheat>();
    }

    void Update()
    {
        if (Time.time - lastClickTime > comboWindow && currentComboIndex > 0)
        {
            ResetCombo();
        }
        
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            ExecuteComboStep();
        }
    }

    void ExecuteComboStep()
    {
        // 1. Animation Logic
        anim.SetInteger("ComboIndex", currentComboIndex);
        anim.SetTrigger("Attack");

        // 2. Mouse & Direction Logic
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 direction = (mousePos - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 3. Rotation & Spawning
        Quaternion finalRotation = Quaternion.Euler(0, 0, angle + rotationOffset);
        GameObject prefabToSpawn = comboPrefabs[currentComboIndex];
        GameObject projectile = Instantiate(prefabToSpawn, firePoint.position, finalRotation);

        // 4. APPLY CHEAT DAMAGE BOOST
        Bullet bulletScript = projectile.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            float multiplier = 1f;

            // Check if the Knight Cheat is active in the PlayerCheat script
            if (cheatSystem != null && cheatSystem.isKnightCheatActive)
            {
                multiplier = cheatSystem.knightDamageBoost;
                
                // Visual Feedback for boosted bullets
                projectile.GetComponent<SpriteRenderer>().color = Color.red;
                projectile.transform.localScale *= 1.3f;
            }

            bulletScript.SetDamageMultiplier(multiplier);
        }

        // 5. Physics Movement
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        // 6. Logic Updates
        lastClickTime = Time.time;
        nextFireTime = Time.time + baseFireRate;
        currentComboIndex++;

        if (currentComboIndex >= comboPrefabs.Count)
        { 
            currentComboIndex = 0; 
        }
    }

    public void ResetCombo()
    {
        currentComboIndex = 0;
        anim.SetInteger("ComboIndex", 0);
        Debug.Log("Kombo sıfırlandı.");
    }
}