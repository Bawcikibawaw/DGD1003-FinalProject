using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // Input System kütüphanesi

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI; // Unity'de oluşturduğumuz Paneli buraya atacağız

    void Update()
    {
        // ESC tuşuna basıldı mı? (New Input System)
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    // --- BUTON FONKSİYONLARI ---

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Menüyü kapat
        Time.timeScale = 1f;          // Zamanı normal akışına döndür
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);  // Menüyü aç
        Time.timeScale = 0f;          // ZAMANI DURDUR (Her şeyi dondurur)
        GameIsPaused = true;
    }

    public void RestartGame()
    {
        // Yeniden başlatırken zamanı düzeltmeyi unutma! Yoksa oyun donuk başlar.
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Oyundan Çıkılıyor...");
        Application.Quit(); // Bu sadece build alınmış (exe) oyunda çalışır.
    }
}