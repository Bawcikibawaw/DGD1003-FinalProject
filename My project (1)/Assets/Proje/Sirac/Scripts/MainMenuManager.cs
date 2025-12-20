using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçişleri için şart

public class MainMenuManager : MonoBehaviour
{
    // "Play" butonuna basınca çalışacak
    public void PlayGame()
    {
        // 1 numaralı sahneye (Oyun Sahnesine) git
        // File -> Build Settings kısmında oyun sahnene 1 numara vermelisin.
        SceneManager.LoadScene(1); 
    }

    // "Quit" butonuna basınca çalışacak
    public void QuitGame()
    {
        Debug.Log("Oyundan Çıkıldı!"); // Editörde çıkmaz, sadece Log verir
        Application.Quit();
    }

    // "Credits" butonu için basit bir Log (İstersen ayrı panele bağlarız)
    public void ShowCredits()
    {
        Debug.Log("Yapımcı: [Senin Adın]");
    }
}