using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Game Data/Player")]
public class PlayerSO : ScriptableObject
{
    public string characterName = "New Character";
    
    //public Sprite characterIcon;
    
    [TextArea(3, 5)]
    public string description = "A brave new adventurer.";
    
    public GameObject characterPrefab;
    
    public int characterID;
    
    public float maxHealth = 100f;
    public float attackPower = 15f;
}
    
