using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    public Transform target;       // Takip edilecek karakter (Menu Player)
    public float smoothSpeed = 0.125f;
    public Vector3 offset;         // Mesafe ayarı

    void LateUpdate()
    {
        if (target == null) return;

        // Hedef pozisyon (X ekseninde biraz sağa kaydırıyoruz ki menü altında kalmasın)
        // Offset değerini Unity'den X: 3 veya 4 yaparak sağa alabilirsin.
        Vector3 desiredPosition = target.position + offset;
        
        // Yumuşak geçiş
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Z eksenini sabitle (-10 standart kamera derinliğidir)
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10f);
    }
}