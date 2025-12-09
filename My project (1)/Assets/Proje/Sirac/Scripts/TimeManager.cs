using UnityEngine;
using UnityEngine.InputSystem; // Yeni Input Sistemi

public class TimeManager : MonoBehaviour
{
    private TimeRewind[] rewindables;

    void Start()
    {
        // Sahnedeki TÜM TimeRewind script'lerini bulur (Player, Enemy, Bullet...)
        rewindables = FindObjectsOfType<TimeRewind>();
    }

    void Update()
    {
        // Not: Eğer oyun sırasında yeni mermiler oluşuyorsa, 
        // bu 'FindObjectsOfType'ı periyodik olarak güncellemek gerekebilir.
        // Ama şimdilik prototip için bu yeterli.

        if (Mouse.current != null)
        {
            // Sağ tuşa BASILDIĞI AN
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                // Listeyi GÜNCELLE (Yeni oluşturulan mermileri de bulsun)
                rewindables = FindObjectsOfType<TimeRewind>();
                StartAllRewinds();
            }
            
            // Sağ tuş BIRAKILDIĞI AN
            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                StopAllRewinds();
            }
        }
    }

    void StartAllRewinds()
    {
        foreach (TimeRewind obj in rewindables)
        {
            if (obj != null) // Obje (mermi gibi) yok olmamışsa
            {
                obj.StartRewind();
            }
        }
    }

    void StopAllRewinds()
    {
        foreach (TimeRewind obj in rewindables)
        {
             if (obj != null) // Obje (mermi gibi) yok olmamışsa
            {
                obj.StopRewind();
            }
        }
    }
}