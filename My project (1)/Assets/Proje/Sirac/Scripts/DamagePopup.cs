using UnityEngine;
using TMPro; // TextMeshPro kullanmak için bu şart!

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    public float moveSpeed = 2f;    // Yukarı çıkma hızı
    public float disappearTime = 1f; // Kaç saniyede kaybolsun?
    private Color textColor;

    // Bu fonksiyonu dışarıdan (Düşmandan) çağıracağız
    public void Setup(int damageAmount)
    {
        textMesh = GetComponent<TextMeshPro>();
        // Sayıyı yazıya çevir
        textMesh.text = damageAmount.ToString();
        
        // Başlangıç rengini al
        textColor = textMesh.color;
    }

    void Update()
    {
        // 1. YUKARI HAREKET
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // 2. YAVAŞÇA KAYBOLMA (Fade Out)
        disappearTime -= Time.deltaTime; // Süreyi azalt
        if (disappearTime < 0)
        {
            // Süre dolunca şeffaflığı (Alpha) azalt
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;

            // Tamamen görünmez olunca objeyi yok et
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}