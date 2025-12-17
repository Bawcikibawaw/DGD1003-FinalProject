using UnityEngine;
public class DestroyAfterTime : MonoBehaviour
{
    public float delay = 1f; // 1 saniye sonra yok ol
    void Start()
    {
        Destroy(gameObject, delay);
    }
}