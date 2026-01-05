using UnityEngine;
using System.Collections;

public class PlayerCheat : MonoBehaviour
{
    private int _id;
    private bool _isInitialized = false;
    private Animator anim;

    [Header("General Settings")]
    public float shieldDuration = 3.0f;
    public float knightDuration = 5.0f;

    [Header("Wizard Ability (ID 0)")]
    public GameObject wizardEffectPrefab; 
    public float verticalOffset = 2.0f;    
    public float wizardEffectDeactivateDelay = 0.5f;

    [Header("Knight Ability (ID 1)")]
    public float knightDamageBoost = 3.0f; 
    public bool isKnightCheatActive = false;

    [Header("Visual Effects")]
    public float cheatZoomSize = 3.5f;
    public float zoomSpeed = 5f;
    public float wizardSlowMoScale = 0.4f; // Only used for Wizard
    private float originalZoomSize;
    private Camera mainCam;

    private Coroutine visualRoutine;

    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
        
        if (mainCam != null)
            originalZoomSize = mainCam.orthographicSize;
    }

    public void InitializeAbility(int characterID)
    {
        _id = characterID;
        _isInitialized = true;
    }

    void Update()
    {
        if (!_isInitialized) return;

        if (Input.GetKeyDown(KeyCode.R) && PlayerMovement.Instance.currentCheat == 100)
        {
            ExecuteAbilityByID(_id);
            PlayerMovement.Instance.currentCheat = 0;
        }
    }

    void ExecuteAbilityByID(int id)
    {
        switch (id)
        {
            case 0: // WIZARD (Cinematic Strike)
                ApplyCheatVisuals(true, true); // Zoom IN + Slow Mo
                anim.SetBool("playerSwitch", true);
                
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    Vector3 spawnPos = enemy.transform.position + Vector3.up * verticalOffset;
                    if (wizardEffectPrefab != null)
                    {
                       GameObject effectInstance = Instantiate(wizardEffectPrefab, spawnPos, Quaternion.identity);
                       Destroy(effectInstance, 1f);
                    }
                    Destroy(enemy);
                }
                Invoke("DeactivateWizardEffect", wizardEffectDeactivateDelay);
                break;
            
            case 1: // KNIGHT (Active Buff)
                ApplyCheatVisuals(true, false); // Zoom IN ONLY
                isKnightCheatActive = true;
                anim.SetBool("playerSwitch", true);
                Invoke("DeactivateKnightMode", knightDuration);
                break;
            
            case 2: // TANK (Active Buff)
                ApplyCheatVisuals(true, false); // Zoom IN ONLY
                PlayerMovement.Instance.isInvincible = true;
                anim.SetBool("playerSwitch", true);
                Invoke("DeactivateGodMode", shieldDuration);
                break;
        }
    }

    // --- VISUAL LOGIC ---

    private void ApplyCheatVisuals(bool active, bool useSlowMo)
    {
        if (visualRoutine != null) StopCoroutine(visualRoutine);
        
        float targetZoom = active ? cheatZoomSize : originalZoomSize;
        
        // Handle Time Scale
        if (active && useSlowMo)
        {
            Time.timeScale = wizardSlowMoScale;
        }
        else
        {
            Time.timeScale = 1f; // Normal speed for Knight/Tank buffs
        }
        
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        visualRoutine = StartCoroutine(TransitionVisuals(targetZoom));
    }

    IEnumerator TransitionVisuals(float targetSize)
    {
        while (Mathf.Abs(mainCam.orthographicSize - targetSize) > 0.01f)
        {
            // Use unscaledDeltaTime so zoom is always smooth regardless of timeScale
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, targetSize, Time.unscaledDeltaTime * zoomSpeed);
            yield return null;
        }
        mainCam.orthographicSize = targetSize;
    }

    // --- DEACTIVATION METHODS ---

    void DeactivateWizardEffect()
    {
        anim.SetBool("playerSwitch", false);
        ApplyCheatVisuals(false, false); // Everything back to normal
    }

    void DeactivateKnightMode()
    {
        isKnightCheatActive = false;
        anim.SetBool("playerSwitch", false);
        ApplyCheatVisuals(false, false); // Everything back to normal
    }

    void DeactivateGodMode()
    {
        PlayerMovement.Instance.isInvincible = false;
        anim.SetBool("playerSwitch", false);
        ApplyCheatVisuals(false, false); // Everything back to normal
    }
}