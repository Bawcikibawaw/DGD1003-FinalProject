using UnityEngine;

[CreateAssetMenu(fileName = "GameSelectionManager", menuName = "Game/Selection Manager")]
public class PlayerSelectionSO : ScriptableObject
{
   
    public PlayerSO selectedCharacter;


    public void SelectCharacter(PlayerSO character)
    {
        selectedCharacter = character;
        Debug.Log("Karakter Seçildi: " + character.characterName);
    }
    
    public bool IsCharacterSelected()
    {
        return selectedCharacter != null;
    }
    
    public void ClearSelection()
    {
        selectedCharacter = null;
    }
}