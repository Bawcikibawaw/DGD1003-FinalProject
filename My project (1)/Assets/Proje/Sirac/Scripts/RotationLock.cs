using UnityEngine;

public class RotationLock : MonoBehaviour
{
    // Bar düşmanın ne kadar yukarısında dursun? (X, Y, Z)
    // Y'yi 1.0f veya 0.8f yaparak yüksekliği ayarla.
    public Vector3 offset = new Vector3(0, 1.2f, 0); 

    void LateUpdate()
    {
        // Önce babamız (Düşman) var mı kontrol edelim
        if (transform.parent != null)
        {
            // 1. DÖNMEYİ KİLİTLE: Hep dümdüz dur
            transform.rotation = Quaternion.identity;

            // 2. POZİSYONU KİLİTLE: 
            // Düşman dönerse bar yan tarafa kaymasın.
            // Barı zorla "Düşman Pozisyonu + Offset" konumuna ışınla.
            transform.position = transform.parent.position + offset;
        }
    }
}