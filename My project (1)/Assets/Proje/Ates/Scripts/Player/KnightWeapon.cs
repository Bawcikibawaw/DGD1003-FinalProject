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

    void Update()
    {
        // Kombo penceresi kontrolü
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
        GameObject prefabToSpawn = comboPrefabs[currentComboIndex];
        
        Quaternion spawnRotation = firePoint.rotation * prefabToSpawn.transform.rotation;
        
        Instantiate(prefabToSpawn, firePoint.position, spawnRotation);
        
        Debug.Log("Atılan Obje: " + prefabToSpawn.name + " | Kombo Adımı: " + (currentComboIndex + 1));

        //Update timer
        lastClickTime = Time.time;
        nextFireTime = Time.time + baseFireRate;

        //Increase combo step
        currentComboIndex++;

        //If combo end reset
        if (currentComboIndex >= comboPrefabs.Count)
        { 
            ResetCombo();
        }
    }

    void ResetCombo()
    {
        currentComboIndex = 0;
        Debug.Log("Kombo sıfırlandı.");
    }
}