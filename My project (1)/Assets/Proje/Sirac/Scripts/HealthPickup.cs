using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 20; 

    void OnTriggerEnter2D(Collider2D other)
    {
        // --- CASUS SATIR ---
        // Kutuya bir şey değdiği an Konsola ismini yazacak.
        Debug.Log("KUTUYA DEĞEN OBJE: " + other.name + " | ETIKETI: " + other.tag);
        // -------------------

        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("Player etiketi doğru ama üzerinde 'PlayerHealth' scripti YOK!");
            }
        }
    }
}