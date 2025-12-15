using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro kullanıyorsan

public class LevelSystem : MonoBehaviour
{
    // Heryerden erişilebilsin diye Singleton yapıyoruz
    public static LevelSystem instance;

    [Header("Level Ayarları")]
    public int currentLevel = 1;
    public float currentXP = 0;
    public float maxXP = 100; // İlk level için gereken XP

    [Header("UI Bağlantıları")]
    public Slider xpSlider;
    public GameObject levelUpPanel; // O gizlediğimiz panel
    
    [Header("Oyuncu Scriptleri")]
    public PlayerMovement playerMoveScript; // Hızlanmak için
    public PlayerHealth playerHealthScript; // Can basmak için
    // public Shooting shootingScript; // Ateş hızını artırmak için (Varsa buraya ekle)

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateUI();
        levelUpPanel.SetActive(false); // Başlarken panel kapalı olsun
    }

    public void AddExperience(float amount)
    {
        currentXP += amount;

        // XP Barını yumuşakça değil direkt güncelleyelim şimdilik
        UpdateUI();

        // Level atlama kontrolü
        if (currentXP >= maxXP)
        {
            LevelUp();
        }
    }

    void UpdateUI()
    {
        if(xpSlider != null)
        {
            xpSlider.maxValue = maxXP;
            xpSlider.value = currentXP;
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP = 0; // XP'yi sıfırla (veya artanı bir sonraki levele devret: currentXP -= maxXP)
        maxXP = maxXP * 1.2f; // Bir sonraki level %20 daha zor olsun
        UpdateUI();

        // OYUNU DURDUR VE PANELİ AÇ
        Time.timeScale = 0f; // Zamanı dondurur
        levelUpPanel.SetActive(true);
    }

    // --- BUTONLARIN ÇAĞIRACAĞI FONKSİYONLAR ---

    public void UpgradeSpeed()
    {
        if (playerMoveScript != null)
        {
            playerMoveScript.moveSpeed += 1f; // Hızı 1 artır
            Debug.Log("Hız Artırıldı!");
        }
        CloseMenuAndResume();
    }

    public void UpgradeHealth()
    {
        if (playerHealthScript != null)
        {
            playerHealthScript.Heal(30); // 30 Can ver
            // Veya max canı artırmak istersen: playerHealthScript.maxHealth += 20;
            Debug.Log("Can Yenilendi!");
        }
        CloseMenuAndResume();
    }

    public void UpgradeFireRate()
    {
        // Burada Shooting scriptine ulaşıp ateş hızını artırabilirsin
        // Örnek: shootingScript.fireRate -= 0.05f; 
        Debug.Log("Saldırı Hızı Artırıldı! (Kodunu bağlaman lazım)");
        CloseMenuAndResume();
    }

    void CloseMenuAndResume()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1f; // Zamanı tekrar başlat
    }
}