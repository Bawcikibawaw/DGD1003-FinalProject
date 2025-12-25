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
                PlayerMovement.Instance.currentHealth += healAmount;
                Destroy(gameObject);
        }
    }
}