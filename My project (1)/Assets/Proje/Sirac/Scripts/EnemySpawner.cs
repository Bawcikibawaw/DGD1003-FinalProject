using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;    // Hangi düşmanı oluşturacağız? (Enemy prefab'ını buraya sürükle)
    public Transform player;          // Kime doğru gidecekler? (Player objesini buraya sürükle)
    public float spawnRate = 2f;      // Kaç saniyede bir yeni düşman gelsin?
    public float spawnDistance = 15f; // Oyuncudan ne kadar uzakta doğsunlar? (Ekran dışı olması için)

    private float nextSpawnTime;      // Bir sonraki doğum için zamanlayıcı

    void Update()
    {
        // Zamanı geldiyse ve oyuncu hayattaysa
        if (Time.time >= nextSpawnTime && player != null)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnRate; // Zamanlayıcıyı bir sonraki doğuma ayarla
        }
    }

    void SpawnEnemy()
    {
        // 1. Oyuncunun etrafında 360 derecelik rastgele bir yön seç
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // 2. O yönde, 'spawnDistance' kadar uzağa bir pozisyon belirle
        Vector2 spawnPosition = (Vector2)player.position + randomDirection * spawnDistance;

        // 3. Düşman prefab'ını o pozisyonda oluştur (Instantiate)
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Quaternion.identity = rotasyon yok

        Debug.Log("Yeni düşman " + spawnPosition + " adresinde doğdu!");
    }
}