using UnityEngine;
using UnityEngine.Audio;
using System; // Array işlemleri için gerekli

public class AudioManager : MonoBehaviour
{
    // Sesleri rahatça ayarlamak için özel bir sınıf
    [System.Serializable]
    public class Sound
    {
        public string name;          // Sesin adı (Örn: "Shoot", "Theme")
        public AudioClip clip;       // Ses dosyası
        [Range(0f, 1f)] public float volume = 0.7f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop;            // Tekrar etsin mi? (Müzik için Evet)

        [HideInInspector] public AudioSource source; // Unity otomatik yönetecek
    }

    public Sound[] sounds; // Inspector'da dolduracağımız liste

    public static AudioManager instance; // Singleton

    void Awake()
    {
        // Singleton Kurulumu (Sahneler arası geçişte yok olmasın)
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject); // Sahne değişse bile müzik kesilmesin

        // Her ses için bir AudioSource bileşeni oluştur
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        // Oyun açılınca hemen arka plan müziğini çal
        Play("Theme"); 
    }

    // Başka scriptlerden çağırmak için kullanacağımız fonksiyon
    public void Play(string name)
    {
        // İsmi arayıp buluyoruz
        Sound s = Array.Find(sounds, sound => sound.name == name);
        
        if (s == null)
        {
            Debug.LogWarning("Ses Bulunamadı: " + name);
            return;
        }
        s.source.Play();
    }
}