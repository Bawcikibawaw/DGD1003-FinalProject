using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUISlot : MonoBehaviour
{
    [Header("Bu Karta Ait Bilgiler")]
    public PlayerSO characterData; // Bu kutuda hangi karakter görünecek?
    
    [Header("UI Bağlantıları")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;
    public Button selectButton; // O kutunun üzerindeki buton

    [Header("Yönetici Bağlantısı")]
    public PlayerSelectionUI uiManager; // Ana yöneticiye haber vereceğiz

    void Start()
    {
        // Başlangıçta bu kart kendi bilgilerini doldursun
        if (characterData != null)
        {
            nameText.text = characterData.characterName;
            descText.text = characterData.description;
            
            // Butona tıklanınca ne olacağını ayarla
            selectButton.onClick.AddListener(OnSlotClicked);
        }
    }

    void OnSlotClicked()
    {
        Debug.Log("Butona tıklandı! İsim: " + name);
        
        // Ana yöneticiye "Ben seçildim!" de.
        uiManager.SelectCharacter(characterData);
        Debug.Log(characterData.characterName + " seçildi.");
    }
}