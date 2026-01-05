using UnityEngine;

public class PlayerCheat : MonoBehaviour
{
    private int _id;
    private bool _isInitialized = false;
    private Animator anim;

    [Header("General Settings")]
    public float shieldDuration = 3.0f;
    public float knightDuration = 5.0f;

    [Header("Wizard Ability (ID 0)")]
    public GameObject wizardEffectPrefab; 
    public float verticalOffset = 2.0f;    
    public float wizardEffectDeactivateDelay = 0.5f;

    [Header("Knight Ability (ID 1)")]
    public float knightDamageBoost = 3.0f; 
    public bool isKnightCheatActive = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Called by GameInitializer to set the character type
    public void InitializeAbility(int characterID)
    {
        _id = characterID;
        _isInitialized = true;
        Debug.Log($"Yetenek Sistemi Hazır. Karakter ID: {_id}");
    }

    void Update()
    {
        if (!_isInitialized) return;

        // Trigger Cheat with 'R' if gauge is full
        if (Input.GetKeyDown(KeyCode.R) && PlayerMovement.Instance.currentCheat == 100)
        {
            ExecuteAbilityByID(_id);
            // Reset the cheat gauge after use
            PlayerMovement.Instance.currentCheat = 0;
        }
    }

    void ExecuteAbilityByID(int id)
    {
        switch (id)
        {
            case 0: // WIZARD: Area Execution
                anim.SetBool("playerSwitch", true);
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                
                foreach (GameObject enemy in enemies)
                {
                    Vector3 spawnPos = enemy.transform.position + Vector3.up * verticalOffset;

                    if (wizardEffectPrefab != null)
                    {
                       GameObject effectInstance = Instantiate(wizardEffectPrefab, spawnPos, Quaternion.identity);
                       Destroy(effectInstance, 1f); // Cleanup effect
                    }
                    Destroy(enemy); // Kill enemy
                }
                
                Debug.Log("Wizard: Tüm düşmanlar yok edildi!");
                Invoke("DeactivateWizardEffect", wizardEffectDeactivateDelay);
                break;
            
            case 1: // KNIGHT: Damage Escalation
                isKnightCheatActive = true;
                anim.SetBool("playerSwitch", true);

                Debug.Log("Knight: DAMAGE ARTTI!");
                Invoke("DeactivateKnightMode", knightDuration);
                break;
            
            case 2: // TANK: God Mode
                PlayerMovement.Instance.isInvincible = true;
                anim.SetBool("playerSwitch", true);

                Debug.Log("Tank: KALKAN AÇILDI!");
                Invoke("DeactivateGodMode", shieldDuration);
                break;
            
            case 3: // THIEF
                Debug.Log("Thief yeteneği henüz eklenmedi.");
                break;
        }
    }

    // --- DEACTIVATION METHODS ---

    void DeactivateWizardEffect()
    {
        anim.SetBool("playerSwitch", false);
    }

    void DeactivateKnightMode()
    {
        isKnightCheatActive = false;
        anim.SetBool("playerSwitch", false);
        Debug.Log("Knight: Hasar normale döndü.");
    }

    void DeactivateGodMode()
    {
        PlayerMovement.Instance.isInvincible = false;
        anim.SetBool("playerSwitch", false);
        Debug.Log("Tank: KALKAN BİTTİ!");
    }
}