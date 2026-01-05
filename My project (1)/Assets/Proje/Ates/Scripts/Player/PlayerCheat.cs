using UnityEngine;
using System.Collections;

public class PlayerCheat : MonoBehaviour
{
    private int _id;
    private bool _isInitialized = false;
    private Animator anim;
    private Collider2D playerCollider; 
    private SpriteRenderer spriteRenderer; // Reference for opacity changes

    [Header("General Settings")]
    public float shieldDuration = 3.0f;
    public float knightDuration = 5.0f;
    public float thiefDuration = 4.0f; 

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
    public float wizardSlowMoScale = 0.4f;
    public float thiefOpacity = 0.5f; // 0.5 means 50% transparent
    private float originalZoomSize;
    private Camera mainCam;

    private Coroutine visualRoutine;

    void Start()
    {
        anim = GetComponent<Animator>();
        mainCam = Camera.main;
        playerCollider = GetComponent<Collider2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>(); // Initialize SpriteRenderer
        
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
            case 0: // WIZARD
                ApplyCheatVisuals(true, true);
                anim.SetBool("playerSwitch", true);
                ExecuteWizardExecution();
                Invoke("DeactivateWizardEffect", wizardEffectDeactivateDelay);
                break;
            
            case 1: // KNIGHT
                ApplyCheatVisuals(true, false);
                isKnightCheatActive = true;
                anim.SetBool("playerSwitch", true);
                Invoke("DeactivateKnightMode", knightDuration);
                break;
            
            case 2: // TANK
                ApplyCheatVisuals(true, false);
                PlayerMovement.Instance.isInvincible = true;
                anim.SetBool("playerSwitch", true);
                Invoke("DeactivateGodMode", shieldDuration);
                break;
            
            case 3: // THIEF
                ApplyCheatVisuals(true, false);
                anim.SetBool("playerSwitch", true);
                
                // Toggle Trigger and Lower Opacity
                if (playerCollider != null) playerCollider.isTrigger = true;
                SetPlayerOpacity(thiefOpacity);
                
                Debug.Log("Thief: Hayalet Modu ve Şeffaflık Aktif!");
                Invoke("DeactivateThiefMode", thiefDuration);
                break;
        }
    }

    // --- LOGIC HELPERS ---

    private void SetPlayerOpacity(float alpha)
    {
        if (spriteRenderer != null)
        {
            Color tempColor = spriteRenderer.color;
            tempColor.a = alpha;
            spriteRenderer.color = tempColor;
        }
    }

    private void ExecuteWizardExecution()
    {
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
    }

    // --- VISUAL LOGIC ---

    private void ApplyCheatVisuals(bool active, bool useSlowMo)
    {
        if (visualRoutine != null) StopCoroutine(visualRoutine);
        
        float targetZoom = active ? cheatZoomSize : originalZoomSize;
        Time.timeScale = (active && useSlowMo) ? wizardSlowMoScale : 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        
        visualRoutine = StartCoroutine(TransitionVisuals(targetZoom));
    }

    IEnumerator TransitionVisuals(float targetSize)
    {
        while (Mathf.Abs(mainCam.orthographicSize - targetSize) > 0.01f)
        {
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, targetSize, Time.unscaledDeltaTime * zoomSpeed);
            yield return null;
        }
        mainCam.orthographicSize = targetSize;
    }

    // --- DEACTIVATION METHODS ---

    void DeactivateWizardEffect()
    {
        anim.SetBool("playerSwitch", false);
        ApplyCheatVisuals(false, false);
    }

    void DeactivateKnightMode()
    {
        isKnightCheatActive = false;
        anim.SetBool("playerSwitch", false);
        ApplyCheatVisuals(false, false);
    }

    void DeactivateGodMode()
    {
        PlayerMovement.Instance.isInvincible = false;
        anim.SetBool("playerSwitch", false);
        ApplyCheatVisuals(false, false);
    }

    void DeactivateThiefMode()
    {
        if (playerCollider != null) playerCollider.isTrigger = false;
        SetPlayerOpacity(1.0f); // Reset to full opacity (100%)
        
        anim.SetBool("playerSwitch", false);
        ApplyCheatVisuals(false, false);
        Debug.Log("Thief: Hayalet Modu Bitti.");
    }
}