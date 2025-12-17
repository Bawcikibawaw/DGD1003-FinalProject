using UnityEngine;

public class PlayerCheat : MonoBehaviour
{
    private int _id;
    private bool _isInitialized = false;
    public float shieldDuration = 0.5f;

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
        if (Input.GetKeyDown(KeyCode.R))
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
                    Destroy(enemy);
                }
                Debug.Log("Terminator saldırısı: Tüm düşmanlar yok edildi!");
                break;
            
            case 1: // Knight

                if (PlayerMovement.Instance.currentCheat == 100)
                {
                    PlayerMovement.Instance.isInvincible = true;

                    Debug.Log("KALKAN AÇILDI!");

                    PlayerMovement.Instance.currentCheat = 0;
                }

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
        Debug.Log("KALKAN BİTTİ!");
    }
}