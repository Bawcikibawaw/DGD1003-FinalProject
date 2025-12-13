using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private float shakeTimeRemaining;
    private float shakePower;
    private float shakeFadeTime;
    
    // Kameranın babasına göre durması gereken orijinal yer (Genelde 0,0,-10)
    private Vector3 initialPosition;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Oyun başlarken kameranın yerel pozisyonunu kaydet
        // (CameraHolder'ın tam ortası ise 0,0,-10 olabilir)
        initialPosition = transform.localPosition;
    }

    void Update() // LateUpdate yerine Update kullanıyoruz ki Follow'dan bağımsız çalışsın
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            // Rastgele bir sapma oluştur
            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            // DİKKAT: Burada 'localPosition' kullanıyoruz!
            // Orijinal yerel pozisyonun üzerine sapmayı ekliyoruz.
            transform.localPosition = initialPosition + new Vector3(xAmount, yAmount, 0f);

            // Gücü yavaşça azalt
            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        }
        else
        {
            // Titreme bittiği an kamerayı ZORLA orijinal yerine ışınla
            // Bu satır kaymayı %100 engeller.
            transform.localPosition = initialPosition;
        }
    }

    public void Shake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;
        shakeFadeTime = power / length;
    }
}