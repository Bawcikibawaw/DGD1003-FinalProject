using UnityEngine;


public class GhostTrait : MonoBehaviour
{
    [Header("Yetenek Ayarları")]
    public float duration = 5f;    
    public float cheatCost = 50f; //  Bu yeteneğin Cheat Bar maliyeti

    private SpriteRenderer sr;
    private PlayerMovement playerMovement; // PlayerMovement scriptini tutar
    private LevelSystem levelSystem;        // LevelSystem scriptini tutar
    private bool isGhostActive = false;     // Yeteneğin aktif olup olmadığını kontrol eder

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // PlayerMovement scriptinin karakter objesinin üzerinde olduğunu varsayıyoruz.
        playerMovement = GetComponent<PlayerMovement>();
        
        // LevelSystem'ı sahnede bul 
        levelSystem = LevelSystem.instance; 
    }

    void Update()
    {
        // Yeteneği kullanmak için H tuşuna basıldığında
        if (Input.GetKeyDown(KeyCode.H) && !isGhostActive)
        {
            // --- BAR KONTROLÜ: LevelSystem üzerinden cheat barda yeterli enerji var mı? ---
            if (levelSystem != null && levelSystem.UseCheat(cheatCost))
            {
                ActivateGhost();
            }
            else if (levelSystem != null)
            {
                Debug.Log("HAYALET MODU: Yetersiz Cheat Gücü! Maliyet: " + cheatCost);
            }
        }
    }

    void ActivateGhost()
    {
        isGhostActive = true;
        
        // 1. Görsel Değişiklik ve Savunma
        if (sr != null) { 
            Color c = sr.color; 
            c.a = 0.3f; 
            sr.color = c; 
        }
        gameObject.tag = "Untagged"; // Düşmanlar 'Player' tagini görmesin

        if (playerMovement != null) playerMovement.isInvincible = true;
        
        // 2. Düşmanların Hedefini Kaybetme
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            // Hedefi NULL yap ki, kovalama bıraksın
            if (ai != null) ai.playerTarget = null; 
        }
        
        Debug.Log("HAYALET MODU AÇIK! (" + duration + " sn)");
        
        // Yetenek süresini başlat
        Invoke("DeactivateGhost", duration);
    }

    void DeactivateGhost()
    {
        isGhostActive = false;

        // 1. Görsel Değişiklik ve Savunma (Normal hale dön)
        if (sr != null) { 
            Color c = sr.color; 
            c.a = 1f; 
            sr.color = c; 
        }
        gameObject.tag = "Player"; // Tag'i geri ver

        if (playerMovement != null) playerMovement.isInvincible = false;

        // 2. Düşmanların Hedefi Geri Kazanması
        Transform playerTransform = transform; // Oyuncunun Transform bileşeni
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            // Hedefi oyuncunun Transform'una geri ayarla
            if (ai != null) ai.playerTarget = playerTransform; 
        }
        
        Debug.Log("HAYALET MODU BİTTİ!");
    }
}