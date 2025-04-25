using UnityEngine;

public class Planet : MonoBehaviour
{
    const float ScaleFactor = 1f / 1000000f;

    public Transform OrbitCenter { get; set; }
    public float OrbitRadiusKm { get; set; }
    public float OrbitSpeedKms { get; set; }

    public float RotationSpeedKms { get; set; }
    public float Angle { get; set; }

    public void SetPlanetRadius(float radius)
    {
        var scaledRadius = radius * ScaleFactor * 50f;
        this.transform.localScale = new Vector3(scaledRadius, scaledRadius, scaledRadius);
    }

    private void Rotate()
    {
        var scaledRotationSpeed = this.RotationSpeedKms * ScaleFactor;
        this.transform.Rotate(Vector3.up, scaledRotationSpeed * Time.deltaTime);
    }

    private void Orbit()
    {
        if (this.OrbitCenter == null)
            return;

        var scaledOrbitSpeed = this.OrbitSpeedKms * ScaleFactor;
        this.Angle += scaledOrbitSpeed * Time.deltaTime;
        var scaledOrbitRadius = this.OrbitRadiusKm * ScaleFactor;
        var x = Mathf.Cos(this.Angle) * scaledOrbitRadius;
        var z = Mathf.Sin(this.Angle) * scaledOrbitRadius;

        this.transform.position = new Vector3(x, this.transform.position.y, z) + this.OrbitCenter.position;

    }

    protected virtual void Update()
    {
        this.Rotate();
        this.Orbit();
    }
}