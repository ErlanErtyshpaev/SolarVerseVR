using UnityEngine;

public class PlanetOrbit : MonoBehaviour
{
    [Header("Orbit settings")]
    public Transform center;          // обычно Sun
    public float radius = 5f;         // радиус орбиты (Unity units)
    public float periodDays = 365f;   // период обращения (в днях)
    public float initialAngleDeg = 0f;
    public bool clockwise = false;

    public float CurrentAngleDeg { get; private set; }

    public void SetPositionByDays(double daysFromEpoch)
    {
        if (!center || periodDays <= 0.0001f) return;

        double cycles = daysFromEpoch / periodDays;
        double angle = (initialAngleDeg + 360.0 * cycles) % 360.0;
        if (clockwise) angle = 360.0 - angle;

        CurrentAngleDeg = (float)angle;
        float rad = Mathf.Deg2Rad * CurrentAngleDeg;

        Vector3 offset = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * radius;
        transform.position = center.position + offset;
        transform.LookAt(center);
    }
}
