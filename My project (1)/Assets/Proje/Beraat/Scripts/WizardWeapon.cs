using UnityEngine;
using System.Collections.Generic;

public class WizardWeapon : MonoBehaviour
{
    [Header("Combo Settings")]
    public List<GameObject> comboPrefabs; 
    public Transform firePoint;
    public float comboWindow = 1.0f; 
    public float baseFireRate = 0.2f; 


    [Header("Rotation Tuning")]
    [Tooltip("Adjust this only if the sprite art looks sideways. Try -90, 0, or 90.")]
    public float rotationOffset = -90f; 

    private int currentComboIndex = 0;
    private float lastClickTime = 0f;
    private float nextFireTime = 0f;

    void Update()
    {
        // Reset combo if time expires
        if (Time.time - lastClickTime > comboWindow && currentComboIndex > 0)
        {
            ResetCombo();
        }
        
        // Shoot input
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            ExecuteComboStep();
        }
    }

    void ExecuteComboStep()
    {
        // 1. Get Mouse Position and Raw Direction
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0; 
        Vector2 fireDir = ((Vector2)mouseWorldPos - (Vector2)firePoint.position).normalized;

        // 2. Calculate the World Angle (Atan2 returns the angle where 0 is Right)
        float lookAngle = Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg;

        // 3. APPLY ROTATION: We use lookAngle + rotationOffset
        // Note: We use Quaternion.Euler to force a clean rotation every time.
        Quaternion spawnRotation = Quaternion.Euler(0, 0, lookAngle + rotationOffset);

        // 4. Instantiate
        GameObject prefabToSpawn = comboPrefabs[currentComboIndex];
        GameObject projectile = Instantiate(prefabToSpawn, firePoint.position, spawnRotation);

        // 5. SET VELOCITY: Using the raw fireDir ensures it always goes to the mouse
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        
        // Logic updates
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
    }
}