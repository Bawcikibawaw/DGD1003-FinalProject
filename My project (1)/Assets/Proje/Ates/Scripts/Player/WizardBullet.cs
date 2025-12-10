using UnityEngine;

public class WizardBullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 3f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Vector2 direction = transform.up; 
        rb.linearVelocity = direction * speed; 

        Destroy(gameObject, lifetime); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject); 
    }
}