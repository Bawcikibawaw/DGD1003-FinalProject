using UnityEngine;
using System.Collections.Generic;

public class KnightWeapon : MonoBehaviour
{
    [Header("Combo Settings")]
    public List<GameObject> comboPrefabs; 
    public Transform firePoint;
    public float comboWindow = 1.0f; 
    public float baseFireRate = 0.2f; 

    private int currentComboIndex = 0;
    private float lastClickTime = 0f;
    private float nextFireTime = 0f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Kombo penceresi aşılırsa sıfırla
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
        // 1. Önce Hangi Kombo Olduğunu Bildir
        anim.SetInteger("ComboIndex", currentComboIndex);
        
        // 2. Sonra Tetikleyiciyi (Trigger) Çalıştır
        anim.SetTrigger("Attack");

        // Prefab oluşturma mantığı
        GameObject prefabToSpawn = comboPrefabs[currentComboIndex];
        Quaternion spawnRotation = firePoint.rotation * prefabToSpawn.transform.rotation;
        Instantiate(prefabToSpawn, firePoint.position, spawnRotation);

        lastClickTime = Time.time;
        nextFireTime = Time.time + baseFireRate;
        
        currentComboIndex++;

        if (currentComboIndex >= comboPrefabs.Count)
        { 
            // Kombo bittiğinde bir sonraki tık için hazırla
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