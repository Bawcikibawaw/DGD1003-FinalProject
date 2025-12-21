using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;       
    public float smoothSpeed = 0.125f; 
    public Vector3 offset;         

    [Header("Harita Sınırları (Clamp)")]
    public float minX;  // Haritanın en sol noktası
    public float maxX;  // Haritanın en sağ noktası
    public float minY;  // Haritanın en alt noktası
    public float maxY;  // Haritanın en üst noktası

    void Start()
    {
        // Eğer target elle atanmadıysa otomatik bul
        if (target == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) target = playerObj.transform;
        }
    }

    void LateUpdate() 
    {
        if (target != null)
        {
            // 1. Gitmek istediğimiz ham pozisyon
            Vector3 desiredPosition = target.position + offset;

            // 2. --- YENİ KISIM: SINIRLAMA (CLAMP) ---
            // Kameranın X ve Y değerlerini belirlediğimiz kutunun içinde tutuyoruz
            float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

            // Sınırlandırılmış yeni hedef pozisyonumuz
            Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

            // 3. Yumuşak geçişi bu yeni sınırlandırılmış pozisyona göre yap
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}