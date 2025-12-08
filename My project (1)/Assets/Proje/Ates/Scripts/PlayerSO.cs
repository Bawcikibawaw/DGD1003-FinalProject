using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Game Data/Player")]
public class PlayerSO : ScriptableObject
{
    public string playerName = "New Player"; 

    public GameObject playerPrefab; 
    
    public int initialHealth = 100;
    
}