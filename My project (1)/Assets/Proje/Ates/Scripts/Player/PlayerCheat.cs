using UnityEngine;

public class PlayerCheat : MonoBehaviour
{
    private int _id;
    private bool _isInitialized = false;
    public float shieldDuration = 0.5f;
    private Animator anim;
    
    public GameObject wizardEffectPrefab; // Drag your effect prefab (lightning, meteor, etc.) here
    public float verticalOffset = 2.0f;    // How high above the enemy to spawn the effect


    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // GameInitializer'dan ID'yi alan fonksiyon
    public void InitializeAbility(int characterID)
    {
        _id = characterID;
        _isInitialized = true;
        Debug.Log($"Yetenek Sistemi Hazır. Karakter ID: {_id}");
    }

    void Update()
    {
        if (!_isInitialized) return;

        // Mouse 0 ile normal kombo/saldırı
        if (Input.GetKeyDown(KeyCode.R) && PlayerMovement.Instance.currentCheat == 100)
        {
            ExecuteAbilityByID(_id);
        }
    }

    void ExecuteAbilityByID(int id)
    {
        switch (id)
        {
            case 0: // Wizard
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    // 1. Calculate spawn position (Enemy position + Up offset)
                    Vector3 spawnPos = enemy.transform.position + Vector3.up * verticalOffset;

                    // 2. Instantiate the prefab
                    if (wizardEffectPrefab != null)
                    {
                       GameObject effectInstance = Instantiate(wizardEffectPrefab, spawnPos, Quaternion.identity);
                       
                       Destroy(effectInstance, 1f);
                    }

                    // 3. Destroy the enemy
                    Destroy(enemy);
                }
                Debug.Log("Terminator saldırısı: Tüm düşmanlar yok edildi!");
                Invoke("DeactivateWizardEffect",0.5f);
                break;
            
            case 1: // Knight
                
                    PlayerMovement.Instance.isInvincible = true;

                    Debug.Log("KALKAN AÇILDI!");

                    PlayerMovement.Instance.currentCheat = 0;
                
                anim.SetBool("playerSwitch", true);

                Invoke("DeactivateGodMode", shieldDuration);
                break;
            
            case 2: // Okçu
                Debug.Log("Okçu hızlı atış yaptı!");
                break;
            
            case 3: // Thief
                Debug.Log("Okçu hızlı atış yaptı!");
                break;
        }
    }
    
    void DeactivateGodMode()
    {
        PlayerMovement.Instance.isInvincible = false;
        anim.SetBool("playerSwitch" , false);
        Debug.Log("KALKAN BİTTİ!");
    }

    void DeactivateWizardEffect()
    {
        anim.SetBool("playerSwitch", false);
    }
}