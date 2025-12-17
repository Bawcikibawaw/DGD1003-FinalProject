using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class LevelSystem : MonoBehaviour

{
    public static LevelSystem instance;

    [Header("Level Ayarları")]
    public int currentLevel = 1;
    public float currentXP = 0;
    public float maxXP = 100;

    [Header("Cheat (Özel Güç) Ayarları")]
    public float currentCheat = 0;
    public float maxCheat = 100; 

    [Header("UI Bağlantıları")]
    public Slider xpSlider;
    public Slider cheatSlider;
    public GameObject levelUpPanel; 
    
    [Header("Oyuncu Scriptleri")]
    public PlayerMovement playerMoveScript; 
    public PlayerHealth playerHealthScript; 

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI(); 
        UpdateCheatUI(); 
        levelUpPanel.SetActive(false); 
    }
    
    // --- XP FONKSİYONLARI ---
    public void AddExperience(float amount)
    {
        currentXP += amount;
        UpdateUI();
        if (currentXP >= maxXP) LevelUp();
    }

    void UpdateUI()
    {
        if(xpSlider != null)
        {
            xpSlider.maxValue = maxXP;
            xpSlider.value = currentXP;
        }
    }

    // --- CHEAT BARINI DOLDURMA ---
    public void AddCheatValue(float amount)
    {
        currentCheat += amount;
        if (currentCheat > maxCheat) currentCheat = maxCheat;
        UpdateCheatUI();
    }

    // --- CHEAT BARINI KULLANMA KONTROLÜ (YENİ EKLENDİ) ---
    public bool UseCheat(float cost)
    {
        if (currentCheat >= cost)
        {
            currentCheat -= cost; // Maliyeti düşür
            UpdateCheatUI();      // Barı güncelle
            return true;          // Kullanım başarılı
        }
        return false;             // Yetersiz enerji
    }
    
    void UpdateCheatUI()
    {
        if (cheatSlider != null)
        {
            cheatSlider.maxValue = maxCheat;
            cheatSlider.value = currentCheat;
        }
    }

    // --- LEVEL UP & UPGRADE FONKSİYONLARI BURADA DEVAM EDER ---
    void LevelUp()
    {
        currentLevel++;
        currentXP = 0; 
        maxXP = maxXP * 1.2f; 
        UpdateUI();

        Time.timeScale = 0f; 
        levelUpPanel.SetActive(true);
    }
    
    public void UpgradeSpeed()
    {
        if (playerMoveScript != null)
        {
            playerMoveScript.moveSpeed += 1f; 
            Debug.Log("Hız Artırıldı!");
        }
        CloseMenuAndResume();
    }
    
    public void UpgradeHealth()
    {
        if (playerHealthScript != null)
        {
            playerHealthScript.Heal(30); 
            Debug.Log("Can Yenilendi!");
        }
        CloseMenuAndResume();
    }

    public void UpgradeFireRate()
    {
        Debug.Log("Saldırı Hızı Artırıldı! (Kodunu bağlaman lazım)");
        CloseMenuAndResume();
    }

    void CloseMenuAndResume()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f; 
    }
}