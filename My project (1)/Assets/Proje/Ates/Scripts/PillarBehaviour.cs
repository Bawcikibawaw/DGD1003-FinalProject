using UnityEngine;
using UnityEngine.Tilemaps;
public class PillarBehaviour : MonoBehaviour
{
    public Tilemap pillarTilemap;
    [Range(0f, 1f)]
    public float transparencyAmount = 0.4f; // 0.4 is a good "ghostly" look

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Get current color, change Alpha, and set it back
            Color c = pillarTilemap.color;
            c.a = transparencyAmount; 
            pillarTilemap.color = c;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset Alpha to 1 (fully visible)
            Color c = pillarTilemap.color;
            c.a = 1f;
            pillarTilemap.color = c;
        }
        else if (other.CompareTag("Enemy"))
        {
            // Reset Alpha to 1 (fully visible)
            Color c = pillarTilemap.color;
            c.a = 1f;
            pillarTilemap.color = c;
        }
    }
}
