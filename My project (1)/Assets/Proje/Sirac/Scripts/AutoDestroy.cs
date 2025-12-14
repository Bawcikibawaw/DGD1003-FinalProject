using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    void Start()
    {
        // 2 saniye sonra bu objeyi sahneden sil
        Destroy(gameObject, 2f);
    }
}