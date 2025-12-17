using UnityEngine;
using UnityEngine.InputSystem; 

public class GodModeTrait : MonoBehaviour
{
    public float duration = 5f;    
    private PlayerMovement movement;
    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) originalColor = sr.color;
    }
    

    void ActivateGodMode()
    {
        if (movement != null) movement.isInvincible = true;
        if (sr != null) sr.color = Color.yellow;
        Debug.Log("KALKAN AÇILDI!");
        Invoke("DeactivateGodMode", duration);
    }

    void DeactivateGodMode()
    {
        if (movement != null) movement.isInvincible = false;
        if (sr != null) sr.color = originalColor;
        Debug.Log("KALKAN BİTTİ!");
    }
}