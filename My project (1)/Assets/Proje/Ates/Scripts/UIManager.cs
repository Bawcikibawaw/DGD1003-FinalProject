using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;

    public Slider cheathBar;
    
    private PlayerMovement playerMovement;
    
    void Update()
    {
        healthBar.value = PlayerMovement.Instance.currentHealth;
        cheathBar.value = PlayerMovement.Instance.currentCheat;
    }
}
