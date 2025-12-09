using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input Sistemi için şart

public class KillAllTrait : MonoBehaviour
{
    void Start()
    {
        // 1. Karakterin rengini KIRMIZI yap (Tehlikeli görünsün)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.red;
        }
        Debug.Log("Terminator Modu Aktif: Düşmanları yok etmek için 'K' tuşuna bas!");
    }

    void Update()
    {
        // 2. Eğer "K" tuşuna basılırsa
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            NukeEnemies();
        }
    }

    void NukeEnemies()
    {
        // Sahnedeki "Enemy" tag'ine sahip BÜTÜN objeleri bul
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Hepsini tek tek yok et
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
            // İstersen burada bir patlama efekti (Instantiate) de oluşturabilirsin
        }

        Debug.Log(enemies.Length + " düşman yok edildi!");
    }
}