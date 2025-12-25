using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;

    public Slider cheathBar;
    
    void Update()
    {
        healthBar.value = PlayerMovement.Instance.currentHealth;
        cheathBar.value = PlayerMovement.Instance.currentCheat;
    }
}
