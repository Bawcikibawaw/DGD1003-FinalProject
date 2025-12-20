using UnityEngine;

public class MenuDemoActor : MonoBehaviour
{
    public float rotateSpeed = 50f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.5f;

    private float nextFireTime;

    void Update()
    {
        // Kendi etrafında yavaşça dön
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);

        // Rastgele ateş et
        if (Time.time >= nextFireTime)
        {
            if (bulletPrefab != null && firePoint != null)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
            nextFireTime = Time.time + fireRate;
        }
    }
}