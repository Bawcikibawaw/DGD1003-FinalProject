using UnityEngine;
using UnityEngine.InputSystem; 

public class GhostTrait : MonoBehaviour
{
    public float duration = 5f;    
    private SpriteRenderer sr;
    private PlayerMovement movement;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.hKey.wasPressedThisFrame)
        {
            // --- BAR KONTROLÜ ---
            if (movement != null && movement.TryUseCheat())
            {
                ActivateGhost();
            }
        }
    }

    void ActivateGhost()
    {
        if (sr != null) { Color c = sr.color; c.a = 0.3f; sr.color = c; }
        gameObject.tag = "Untagged";

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null) ai.playerTarget = null; 
        }
        
        if (movement != null) movement.isInvincible = true;
        Debug.Log("HAYALET MODU AÇIK!");
        Invoke("DeactivateGhost", duration);
    }

    void DeactivateGhost()
    {
        if (sr != null) { Color c = sr.color; c.a = 1f; sr.color = c; }
        gameObject.tag = "Player";
        if (movement != null) movement.isInvincible = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null) ai.playerTarget = transform; 
        }
        Debug.Log("HAYALET MODU BİTTİ!");
    }
}