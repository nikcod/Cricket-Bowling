using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BallPathDrawer : MonoBehaviour
{
    public float pointSpacing = 0.05f;

    LineRenderer line;
    Vector3 lastPoint;
    bool isDrawing;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        isDrawing = false;
    }

    void Update()
    {
        if (!isDrawing) return;

        if (Vector3.Distance(lastPoint, transform.position) >= pointSpacing)
        {
            AddPoint(transform.position);
        }
    }

    public void StartDrawing()
    {
        line.positionCount = 0;
        lastPoint = transform.position;
        AddPoint(lastPoint);
        isDrawing = true;
    }

    public void StopAndClear()
    {
        isDrawing = false;
        line.positionCount = 0;
    }

    void AddPoint(Vector3 point)
    {
        line.positionCount++;
        line.SetPosition(line.positionCount - 1, point);
        lastPoint = point;
    }
}
