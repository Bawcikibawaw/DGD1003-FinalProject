using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Slider healthBar;

    public Slider cheathBar;
    
    private PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }
    
    void Update()
    {
        healthBar.value = playerMovement.currentHealth;
        cheathBar.value = playerMovement.currentCheat;
    }
}
