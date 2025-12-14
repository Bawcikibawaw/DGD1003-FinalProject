using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Texture2D nisanResmi; // Nişangah resmini buraya atacağız

    void Start()
    {
        // İmleci değiştirme kodu
        // İmlecin TAM ORTASI tıklama noktası olsun diye hesap yapıyoruz:
        Vector2 cursorHotspot = new Vector2(nisanResmi.width / 2, nisanResmi.height / 2);

        // İmleci ayarla
        Cursor.SetCursor(nisanResmi, cursorHotspot, CursorMode.Auto);
    }
}