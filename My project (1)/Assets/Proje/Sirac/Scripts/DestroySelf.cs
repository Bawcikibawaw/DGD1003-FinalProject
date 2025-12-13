using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float lifetime = 10f; // 10 saniye yerde kalsın

    void Start()
    {
        // 10 saniye sonra bu objeyi yok et
        Destroy(gameObject, lifetime);
    }
}