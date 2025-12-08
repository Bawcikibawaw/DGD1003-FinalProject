using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSelectionManager", menuName = "Game Data/Player Selection Manager")]
public class PlayerSelectionSO : ScriptableObject
{
    
    public PlayerSO selectedPlayer; 
    
    public void SelectPlayer(PlayerSO player)
    {
        selectedPlayer = player;
        Debug.Log($"Selected Player: {player.playerName}");
    }
}