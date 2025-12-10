using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectionUI : MonoBehaviour
{
    public PlayerSelectionSO gameSelectionManager; // Veriyi tutan SO
    public Button startButton; // Oyuna Başla butonu (En alttaki)

    private void Start()
    {
        // Başlangıçta "Başla" butonu kapalı olsun (Karakter seçilmedi)
        startButton.interactable = false;
        startButton.onClick.AddListener(StartGame);
    }

    // Bu fonksiyonu artık CharacterSlot scriptleri çağıracak
    public void SelectCharacter(PlayerSO selectedData)
    {
        // Seçimi kaydet
        gameSelectionManager.SelectCharacter(selectedData);
        
        // Artık bir karakter seçildiği için Start butonunu aç
        startButton.interactable = true;
    }

    public void StartGame()
    {
        if (gameSelectionManager.IsCharacterSelected())
        {
            SceneManager.LoadScene("GameScene");
        }
    }
}