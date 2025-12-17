using UnityEngine;
using System.Collections;

public class Shell : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Fırlama Ayarları")]
    public float minForce = 150f;
    public float maxForce = 250f;
    public float torque = 10f; // Dönme hızı

    [Header("Görsel Ayarlar")]
    public float lifetime = 2f; // Ne kadar süre yerde kalsın?
    public float fadeSpeed = 2f; // Yok olurken ne kadar hızla şeffaflaşsın?

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 1. Rastgele bir fırlatma gücü belirle
        float force = Random.Range(minForce, maxForce);

        // 2. Kovanı yana doğru (Sağa) fırlat + hafif yukarı/aşağı sapma ekle
        // (transform.right = Objeye göre sağ taraf)
        Vector2 forceDirection = transform.right * force;
        
        // Hafif rastgelelik ekle ki hepsi aynı noktaya düşmesin
        forceDirection += (Vector2)transform.up * Random.Range(-30f, 30f);

        rb.AddForce(forceDirection);

        // 3. Kovanı döndür (Torque)
        rb.AddTorque(Random.Range(-torque, torque));

        // 4. Yok olma sürecini başlat
        StartCoroutine(FadeAndDestroy());
    }

    IEnumerator FadeAndDestroy()
    {
        // Ömür süresi kadar bekle
        yield return new WaitForSeconds(lifetime);

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        float percent = 0;

        // Yavaşça şeffaflaş (Fade Out)
        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            float alpha = Mathf.Lerp(1, 0, percent);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}