using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{

    public PlayerSelectionSO selectionManager; 

    void Start()
    {
        PlayerSO selectedData = selectionManager.selectedPlayer; 

        if (selectedData != null)
        {
            Debug.Log($"Game started with: {selectedData.playerName}");
            
            if (selectedData.playerPrefab != null)
            {
                Instantiate(selectedData.playerPrefab, Vector3.zero, Quaternion.identity); 
            }

            // 2. Oyuncu Verilerini Kullanma (Örnek)
            // Oyuncu kontrolcüsü (PlayerController) scriptine
            // başlangıç can ve hız değerlerini aktarabilirsiniz.

            // Örneğin:
            // PlayerController player = FindObjectOfType<PlayerController>(); 
            // if (player != null)
            // {
            //     player.SetStats(selectedData.initialHealth, selectedData.movementSpeed);
            // }
        }
        else
        {
            Debug.LogError("No player was selected! Returning to main menu.");
            SceneManager.LoadScene("MainMenu"); 
        }
    }
}