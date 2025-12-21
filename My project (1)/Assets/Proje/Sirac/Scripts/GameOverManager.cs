using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // TextMeshPro (Yazılar) için gerekli

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverPanel; // Panel objesi
    public TextMeshProUGUI waveText; // "Wave Reached: 5" yazısı

    public static GameOverManager instance;

    void Awake()
    {
        instance = this;
    }

    // Oyuncu ölünce bu fonksiyon çalışacak
    public void TriggerGameOver(int currentWave)
    {
        // Paneli aç
        gameOverPanel.SetActive(true);
        
        // Yazıyı İngilizce güncelle
        if(waveText != null)
            waveText.text = "WAVE REACHED: " + currentWave.ToString();
            
        // Oyunu dondur
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Zamanı düzelt
        // Sahneyi yeniden yükle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        // Ana menü sahnen varsa adını buraya yaz ("MainMenu")
        // Şimdilik oyunu yeniden başlatsın:
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}