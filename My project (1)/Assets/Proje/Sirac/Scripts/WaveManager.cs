using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Listeleri kullanmak için
using TMPro;

public class WaveManager : MonoBehaviour
{
    // --- 1. YENİ YAPILAR ---
    
    // Her bir düşman türü ve sayısı için küçük kutucuk
    [System.Serializable]
    public class WaveEnemy
    {
        public GameObject enemyPrefab; // Hangi düşman?
        public int count;            // Bundan kaç tane olsun?
    }

    // Bir Dalganın (Wave) genel ayarları
    [System.Serializable]
    public class Wave
    {
        public string waveName;      // Örn: "Karışık Saldırı"
        public WaveEnemy[] enemies;  // BU DALGADA GELECEK TÜRLER LİSTESİ
        public float rate;           // Saniyede kaç düşman doğsun?
    }

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [Header("Dalga Ayarları")]
    public Wave[] waves;             // Tüm dalgaların listesi
    public Transform[] spawnPoints;  // Haritadaki spawn noktaları
    public float timeBetweenWaves = 5f;

    [Header("UI Ayarları")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI countdownText;

    private int nextWave = 0;
    private float waveCountdown;
    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
        UpdateWaveUI();
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
            
            if(countdownText != null)
                countdownText.text = "Sonraki Dalga: " + Mathf.Round(waveCountdown).ToString();
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Dalga Tamamlandı!");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("TÜM DALGALAR BİTTİ! BAŞA DÖNDÜ.");
        }
        else
        {
            nextWave++;
        }
        
        UpdateWaveUI();
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        
        if(countdownText != null) countdownText.text = "SALDIRI BAŞLADI!";

        // --- TORBA SİSTEMİ ---
        List<GameObject> spawnPool = new List<GameObject>();

        foreach (WaveEnemy entry in _wave.enemies)
        {
            for (int i = 0; i < entry.count; i++)
            {
                spawnPool.Add(entry.enemyPrefab);
            }
        }

        while (spawnPool.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnPool.Count);
            GameObject enemyToSpawn = spawnPool[randomIndex];

            SpawnEnemy(enemyToSpawn);

            spawnPool.RemoveAt(randomIndex);

            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }

    void SpawnEnemy(GameObject _enemy)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("HATA: Spawn Points listesi boş! Inspector'dan nokta ekle.");
            return;
        }

        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        
        // --- DÜZELTİLEN SATIR BURASI ---
        // _sp.rotation YERİNE Quaternion.identity YAZDIK.
        // Artık spawn noktası yamuk olsa bile düşman DÜZ (0 derece) doğacak.
        Instantiate(_enemy, _sp.position, Quaternion.identity);
    }
    
    void UpdateWaveUI()
    {
        if(waveText != null)
            waveText.text = "WAVE " + (nextWave + 1);
    }
}