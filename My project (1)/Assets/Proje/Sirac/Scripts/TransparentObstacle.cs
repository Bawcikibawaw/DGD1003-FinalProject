using UnityEngine;

public class TransparentObstacle : MonoBehaviour
{
    [Header("Ayarlar")]
    [Range(0, 1)] public float transparencyAmount = 0.5f;
    public float fadeSpeed = 5f;

    private SpriteRenderer spriteRenderer;
    private float targetAlpha = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("HATA: Bu objede SpriteRenderer yok! Script yanlış yere atılmış.");
        }
    }

    void Update()
    {
        if (spriteRenderer != null)
        {
            Color currentColor = spriteRenderer.color;
            float newAlpha = Mathf.Lerp(currentColor.a, targetAlpha, fadeSpeed * Time.deltaTime);
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Çarpışan objenin adını konsola yazdır
        Debug.Log("Bir şey çarptı: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("OYUNCU GİRDİ! Şeffaflaşıyor...");
            targetAlpha = transparencyAmount;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("OYUNCU ÇIKTI! Düzeliyor...");
            targetAlpha = 1f;
        }
    }
}