using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    public float moveSpeed = 2f;
    public float disappearTime = 1f;
    private Color textColor;

    // YENİ SETUP FONKSİYONU (isCritical eklendi)
    public void Setup(int damageAmount, bool isCritical)
    {
        textMesh = GetComponent<TextMeshPro>();
        textMesh.text = damageAmount.ToString();

        if (isCritical)
        {
            // --- KRİTİKSE ---
            textMesh.fontSize += 3; // Yazıyı büyüt
            textMesh.color = Color.red; // Kıpkırmızı yap
            textMesh.fontStyle = FontStyles.Bold; // Kalın yap
        }
        else
        {
            // --- NORMAL ---
            textMesh.color = Color.yellow; // Normal sarı (veya turuncu)
        }
        
        textColor = textMesh.color;
    }

    void Update()
    {
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        disappearTime -= Time.deltaTime;
        if (disappearTime < 0)
        {
            float fadeSpeed = 3f;
            textColor.a -= fadeSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}