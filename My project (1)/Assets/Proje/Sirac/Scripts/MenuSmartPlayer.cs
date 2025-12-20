using UnityEngine;

public class MenuSmartPlayer : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 4f;
    public float arenaRadius = 6f; // Merkezden ne kadar uzaklaşabilir?
    public float stopDistance = 3f; // Düşmana ne kadar yaklaşsın?
    public float retreatDistance = 2f; // Ne kadar yakına gelirse kaçsın?

    [Header("Savaş Ayarları")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.4f;
    public float aimAccuracy = 10f; // Düşmanla açı farkı 10 dereceden azsa ateş et

    private Rigidbody2D rb;
    private Vector3 startPos; 
    private float nextFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position; 
    }

    void Update()
    {
        GameObject target = FindBestTarget();

        // 1. ARENA SINIRI KONTROLÜ (Merkezden çok uzaklaştıysa dön)
        if (Vector2.Distance(transform.position, startPos) > arenaRadius)
        {
            Vector2 dirToCenter = (startPos - transform.position).normalized;
            Move(dirToCenter);
            RotateTowards(startPos);
        }
        // 2. DÜŞMAN VARSA SAVAŞ
        else if (target != null)
        {
            float distToEnemy = Vector2.Distance(transform.position, target.transform.position);
            Vector2 dirToEnemy = (target.transform.position - transform.position).normalized;

            // --- Hareket Mantığı (Titremeyi Önler) ---
            if (distToEnemy > stopDistance)
            {
                Move(dirToEnemy); // Yaklaş
            }
            else if (distToEnemy < retreatDistance)
            {
                Move(-dirToEnemy); // Çok dibime girdi, geri kaç
            }
            else
            {
                rb.linearVelocity = Vector2.zero; // İdeal mesafedeyim, dur ve nişan al
            }

            // --- Dönüş ve Ateş ---
            RotateTowards(target.transform.position);

            // SADECE NAMLU DÜŞMANA BAKIYORSA ATEŞ ET
            // (Bakış açım ile düşman arasındaki açı farkını hesapla)
            float angleToEnemy = Vector2.Angle(transform.up, dirToEnemy);
            
            if (angleToEnemy < aimAccuracy) 
            {
                Shoot();
            }
        }
        // 3. HEDEF YOKSA (Devriye / Bekleme)
        else
        {
            rb.linearVelocity = Vector2.zero;
            // Hafifçe etrafa bak
            transform.Rotate(0, 0, 20 * Time.deltaTime);
        }
    }

    void Move(Vector2 dir)
    {
        // Lerp kullanarak yumuşak hızlanma/yavaşlama
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, dir * moveSpeed, Time.deltaTime * 5f);
    }

    void RotateTowards(Vector3 targetPos)
    {
        Vector2 dir = targetPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        // Yumuşak dönüş (Sert dönmez)
        float smoothAngle = Mathf.LerpAngle(rb.rotation, angle, 10f * Time.deltaTime);
        rb.rotation = smoothAngle;
    }

    void Shoot()
    {
        if (Time.time >= nextFireTime && bulletPrefab != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }

    GameObject FindBestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject bestTarget = null;
        float closestDist = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            if (enemy == null) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                bestTarget = enemy;
            }
        }
        return bestTarget;
    }
    
    // Editörde alanı görmek için
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPos != Vector3.zero ? startPos : transform.position, arenaRadius);
    }
}