using UnityEngine;

public class MenuSpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Menü Düşmanı (MenuChaserEnemy olan)
    public float spawnRate = 1.5f; // Ne sıklıkla doğsun?
    public int maxEnemies = 6;     // Ekranda en çok kaç düşman olsun?
    
    // Doğma alanı (Kameranın gördüğü alan kadar olmalı)
    public Vector2 spawnAreaSize = new Vector2(8, 5); 

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            // Sahnedeki düşman sayısını kontrol et
            int count = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (count < maxEnemies)
            {
                Spawn();
            }
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void Spawn()
    {
        // Spawner'ın olduğu yerin etrafında rastgele bir kare içinde doğur
        Vector2 randomPos = new Vector2(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2)
        );

        Vector3 spawnLoc = transform.position + (Vector3)randomPos;
        Instantiate(enemyPrefab, spawnLoc, Quaternion.identity);
    }

    // Editörde alanı görebilmek için çizim (Gizmo)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaSize.x, spawnAreaSize.y, 1));
    }
}