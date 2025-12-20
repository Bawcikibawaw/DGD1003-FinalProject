using UnityEngine;

public class MenuNaturalPlayer : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;
    public float runAwayDistance = 4f; // Düşman bu kadar yaklaşırsa kaçmaya başla
    public float arenaLimit = 7f;      // Merkezden en fazla bu kadar uzaklaş

    [Header("Savaş Ayarları")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;
    public float aimSpeed = 15f;       // Dönüş hızı

    private Rigidbody2D rb;
    private Vector3 centerPoint;       // Arenanın merkezi (Başlangıç noktası)
    private float nextFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        centerPoint = transform.position; // Başladığı yeri merkez kabul et
    }

    void Update()
    {
        GameObject closestEnemy = FindClosestEnemy();
        
        // --- 1. HAREKET MANTIĞI (Vektörel Hesaplama) ---
        Vector2 moveDir = Vector2.zero;

        if (closestEnemy != null)
        {
            float dist = Vector2.Distance(transform.position, closestEnemy.transform.position);

            // Eğer düşman çok yakındaysa -> KAÇ (Ters Vektör)
            if (dist < runAwayDistance)
            {
                Vector2 dirToEnemy = (closestEnemy.transform.position - transform.position).normalized;
                moveDir = -dirToEnemy; // Düşmanın tam tersine git
            }
            // Eğer düşman uzaktaysa ama ekran dışına çıkmıyorsam -> HAFİFÇE YAKLAŞ (Agresiflik)
            else
            {
                // Biraz rastgele hareket ekle ki robot gibi durmasın
                moveDir = (closestEnemy.transform.position - transform.position).normalized * 0.5f; 
            }
        }

        // --- 2. MERKEZ KONTROLÜ (Leash / Tasma Mantığı) ---
        // Eğer merkezden çok uzaklaştıysa, hareket vektörünü merkeze doğru bük
        float distToCenter = Vector2.Distance(transform.position, centerPoint);
        if (distToCenter > arenaLimit)
        {
            Vector2 dirToCenter = (centerPoint - transform.position).normalized;
            // Mevcut hareket isteği ile merkeze dönme isteğini harmanla
            moveDir += dirToCenter * 2f; 
        }

        // Hareketi Uygula (Yumuşak geçişli)
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, moveDir.normalized * moveSpeed, Time.deltaTime * 5f);


        // --- 3. SAVAŞ MANTIĞI ---
        if (closestEnemy != null)
        {
            // Düşmana Dön
            Vector2 dirToTarget = closestEnemy.transform.position - transform.position;
            float angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90f;
            
            // Yumuşak dönüş
            rb.rotation = Mathf.LerpAngle(rb.rotation, angle, aimSpeed * Time.deltaTime);

            // Sadece namlu düşmana bakıyorsa ateş et (Açı farkı 15 dereceden azsa)
            if (Vector2.Angle(transform.up, dirToTarget) < 15f)
            {
                Shoot();
            }
        }
        else
        {
            // Düşman yoksa yavaşla
             rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, Time.deltaTime);
        }
    }

    void Shoot()
    {
        if (Time.time >= nextFireTime && bulletPrefab != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDst = Mathf.Infinity;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < minDst) { minDst = d; closest = e; }
        }
        return closest;
    }

    // Editörde sınırları gör
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(centerPoint != Vector3.zero ? centerPoint : transform.position, arenaLimit);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, runAwayDistance);
    }
}