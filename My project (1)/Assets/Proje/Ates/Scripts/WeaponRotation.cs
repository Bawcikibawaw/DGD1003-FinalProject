using UnityEngine;

public class WeaponRotation : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        
        Vector2 direction = mousePosition - transform.position;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Sprite sağa bakıyorsa: 
        //transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // VEYA
        // Eğer sprite yukarı bakıyorsa (çoğu 2D oyun için yaygındır):
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        
        // VEYA
        // Eğer sprite sağa bakıyorsa ve WeaponModel ters dönüyorsa:
        // transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
    }
}