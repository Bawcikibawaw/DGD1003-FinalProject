using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    public float gorunmeSuresi = 0.1f;
    private Image markerImage;

    void Start()
    {
        markerImage = GetComponent<Image>();
        markerImage.enabled = false; // Başta gizle
    }

    public void Show()
    {
        // 1. ÖNCE KONUMLAN: X işaretini o anki mouse pozisyonuna taşı
        transform.position = Input.mousePosition;

        // 2. SONRA GÖRÜN
        markerImage.enabled = true;

        // 3. ZAMANLA KAPAN
        CancelInvoke("Gizle");
        Invoke("Gizle", gorunmeSuresi);
    }

    void Gizle()
    {
        markerImage.enabled = false;
    }
}