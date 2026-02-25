using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitLine : MonoBehaviour
{
    public Transform center;
    public float radius = 5f;
    public int segments = 128;

    void Start()
    {
        Draw();
    }

    void OnValidate()
    {
        Draw();
    }

    void Draw()
    {
        var lr = GetComponent<LineRenderer>();
        if (!center || radius <= 0 || segments < 8) return;

        lr.useWorldSpace = true;
        lr.loop = true;
        lr.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float t = (float)i / segments * Mathf.PI * 2f;
            Vector3 pos = center.position + new Vector3(Mathf.Cos(t), 0f, Mathf.Sin(t)) * radius;
            lr.SetPosition(i, pos);
        }
    }
}
