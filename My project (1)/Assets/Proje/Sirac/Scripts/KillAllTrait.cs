using UnityEngine;
using UnityEngine.InputSystem; 

public class KillAllTrait : MonoBehaviour
{
    private PlayerMovement movement;

    void Start()
    {
        movement = GetComponent<PlayerMovement>(); // PlayerMovement'a eriş
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.red;
    }
    
    void NukeEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
        Debug.Log("Terminator saldırısı: Tüm düşmanlar yok edildi!");
    }
}