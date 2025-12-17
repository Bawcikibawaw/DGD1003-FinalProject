using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    // Inspector'dan atılacak Yönetici SO
    public PlayerSelectionSO gameSelectionManager;
    
    // Karakterin nerede belireceğini belirleyen Transform (isteğe bağlı)
    public Transform playerSpawnPoint; 

    void Start()
    {
        // Yönetici SO'da bir karakter seçili mi kontrol et
        if (gameSelectionManager.IsCharacterSelected())
        {
            PlayerSO selectedChar = gameSelectionManager.selectedCharacter;

            Debug.Log($"Oyun Başladı! Seçilen Karakter: {selectedChar.characterName}");
            
            GameObject playerPrefab = selectedChar.characterPrefab;
            
            if (playerPrefab != null)
            {
                // Karakteri dünyada Instantiate et (Oluştur)
                Vector3 spawnPosition = playerSpawnPoint != null ? playerSpawnPoint.position : Vector3.zero;
                
                GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                playerInstance.name = selectedChar.characterName + " (Oyuncu)";
                
                PlayerCheat abilityManager = playerInstance.GetComponent<PlayerCheat>();
                if (abilityManager != null)
                {
                    abilityManager.InitializeAbility(selectedChar.characterID);
                }

// 2. Eğer KnightWeapon kullanıyorsan onu da tetikle (ID'ye göre prefabları içeride tutabilirsin)
                KnightWeapon weapon = playerInstance.GetComponent<KnightWeapon>();
                if (weapon != null)
                {
                    // weapon.Initialize(...) 
                }

                // Örneğin, karakterin canını buradan ayarlayabilirsiniz:
                // playerInstance.GetComponent<PlayerStats>().Initialize(selectedChar.maxHealth, selectedChar.attackPower);

                // Seçim bilgisini temizleyerek Main Menu'ye dönüldüğünde tekrar seçilmesini sağlayabilirsiniz.
                // gameSelectionManager.ClearSelection();
            }
            else
            {
                Debug.LogError($"'{selectedChar.characterName}' için karakter prefab'ı atanmamış!");
            }
        }
        else
        {
            // Güvenlik: Eğer direkt oyun sahnesine geçildiyse
            Debug.LogError("Karakter seçimi yapılmadan oyun başlatıldı! Ana Menüye yönlendiriliyor...");
            // SceneManager.LoadScene("MainMenuSceneAdi"); // Scene adını düzenleyin!
        }
    }
}