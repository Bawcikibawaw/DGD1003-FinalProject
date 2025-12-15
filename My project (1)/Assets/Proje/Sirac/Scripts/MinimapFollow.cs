using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player; // Oyuncuyu buraya sürükleyeceğiz

    // Kameranın ne kadar yüksekte duracağı (2D'de Z ekseni)
    // Mevcut Z değerini korumak için bunu Start'ta alacağız.
    private float zPosition;

    void Start()
    {
        zPosition = transform.position.z;
    }

    // Kamera takibi, oyuncu hareket ettikten sonra yapılmalı (LateUpdate)
    void LateUpdate()
    {
        if (player != null)
        {
            // Kameranın yeni pozisyonu:
            // X ve Y = Oyuncunun olduğu yer
            // Z = Kameranın kendi yüksekliği (değişmemeli)
            Vector3 newPosition = player.position;
            newPosition.z = zPosition;

            transform.position = newPosition;

            // İstersen harita oyuncuyla dönmesin (Sabit Kuzey) diye rotasyonu kitleyebilirsin:
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}