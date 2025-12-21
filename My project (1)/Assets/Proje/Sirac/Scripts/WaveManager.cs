using UnityEngine;
using System.Collections;
using TMPro; // UI (Yazı) için gerekli

public class WaveManager : MonoBehaviour
{
    // Wave (Dalga) Özellikleri için bir sınıf
    [System.Serializable]
    public class Wave
    {
        public string waveName;      // Örn: "Isinma Turu"
        public GameObject enemyPrefab; // Hangi düşman gelecek?
        public int count;            // Kaç tane gelecek?
        public float rate;           // Ne sıklıkla doğacak? (Saniye)
    }

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [Header("Dalga Ayarları")]
    public Wave[] waves;             // Inspector'dan ayarlayacağımız dalgalar
    public Transform[] spawnPoints;  // Düşmanların çıkacağı noktalar
    public float timeBetweenWaves = 5f; // İki dalga arası bekleme süresi

    [Header("UI Ayarları")]
    public TextMeshProUGUI waveText;      // "Wave: 1" yazısı
    public TextMeshProUGUI countdownText; // "Sonraki Dalga: 3..." yazısı

    private int nextWave = 0;
    private float waveCountdown;
    private float searchCountdown = 1f;   // Performans için saniyede 1 kere düşman ara
    private SpawnState state = SpawnState.COUNTING;

    void Start()
    {
        waveCountdown = timeBetweenWaves;
        UpdateWaveUI();
    }

    void Update()
    {
        // 1. DÜŞMANLARI KONTROL ET (WAITING MODU)
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return; // Düşmanlar yaşıyorsa bekle, başka işlem yapma
            }
        }

        // 2. GERİ SAYIM YAP
        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                // Geri sayım bitti, doğurmaya başla
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
            
            // UI Güncelle (Geri sayım)
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
            nextWave = 0; // Tüm dalgalar bitti! Başa sar veya oyun bitti ekranı koy.
            Debug.Log("TÜM DALGALAR BİTTİ! DÖNGÜ BAŞA DÖNDÜ.");
            
            // İstersen burada zorluğu artırabilirsin (Multiplier)
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
        
        // Her karede GameObject.Find yapmak çok yorar, saniyede 1 kere bakıyoruz
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f; // Sayacı sıfırla
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false; // Kimse kalmadı
            }
        }
        return true; // Hala düşman var
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;
        
        if(countdownText != null) countdownText.text = "SALDIRI BAŞLADI!";

        // Belirlenen sayı kadar düşman doğur
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemyPrefab);
            yield return new WaitForSeconds(1f / _wave.rate); // Bekle
        }

        state = SpawnState.WAITING; // Doğurma bitti, hepsinin ölmesini bekle
        yield break;
    }

    void SpawnEnemy(GameObject _enemy)
    {
        // Rastgele bir doğuş noktası seç
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
    
    void UpdateWaveUI()
    {
        if(waveText != null)
            waveText.text = "WAVE " + (nextWave + 1);
    }
}