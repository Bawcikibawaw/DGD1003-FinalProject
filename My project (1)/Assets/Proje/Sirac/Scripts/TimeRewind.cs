using System.Collections.Generic;
using UnityEngine;

public class TimeRewind : MonoBehaviour
{
    public float recordTime = 3f; 

    private List<PointInTime> positionHistory; 
    private Rigidbody2D rb;
    private bool isRewinding = false; 

    void Start()
    {
        positionHistory = new List<PointInTime>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }

    void Record()
    {
        if (rb.isKinematic)
        {
            return;
        }

        if (positionHistory.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            positionHistory.RemoveAt(0);
        }

        positionHistory.Add(new PointInTime(transform.position, transform.rotation));
    }

    void Rewind()
    {
        if (positionHistory.Count > 0)
        {
            PointInTime point = positionHistory[positionHistory.Count - 1];
            
            rb.MovePosition((Vector2)point.position); 
            rb.MoveRotation(point.rotation.eulerAngles.z);
            
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true; 
        rb.linearVelocity = Vector2.zero; 
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false; 
    }

    // Kontrol fonksiyonu
    public bool IsRewinding()
    {
        return isRewinding;
    }
}

// Bu yardımcı sınıfın script'in altında olduğundan emin ol
public class PointInTime
{
    public Vector3 position;
    public Quaternion rotation;

    public PointInTime(Vector3 _pos, Quaternion _rot)
    {
        position = _pos;
        rotation = _rot;
    }
}