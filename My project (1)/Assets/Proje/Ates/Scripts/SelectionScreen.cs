using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionScreenManager : MonoBehaviour
{

    public PlayerSelectionSO selectionManager; 

    
    public void OnPlayerSelected(PlayerSO player)
    {
        selectionManager.SelectPlayer(player); 
        
        SceneManager.LoadScene("GameScene"); 
    }
}