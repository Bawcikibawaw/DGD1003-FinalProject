using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;       // Takip edilecek hedef (Player)
    public float smoothSpeed = 0.125f; // Yumuşaklık ayarı (0 ile 1 arası)
    public Vector3 offset;         // Kamera ile oyuncu arasındaki mesafe farkı


    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void LateUpdate() // Kamera işlemleri için LateUpdate en iyisidir (titremeyi önler)
    {
        // Hedefimiz (oyuncu) hala hayattaysa
        if (target != null)
        {
            // Gitmek istediğimiz son pozisyon: Oyuncunun yeri + aradaki fark
            Vector3 desiredPosition = target.position + offset;

            // Şu anki yerden gitmek istediğimiz yere yumuşakça kay (Lerp)
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Kamerayı yeni pozisyona taşı
            transform.position = smoothedPosition;
        }
    }
}