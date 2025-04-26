using UnityEngine;

public class Planet : MonoBehaviour
{
    public float SizeScaleFactor { get; set; }
    public float SpeedScaleFactor { get; set; }

    public Transform OrbitCenter { get; set; }
    public float OrbitRadiusKm { get; set; }
    public float OrbitSpeedKms { get; set; }

    public float RotationSpeedKms { get; set; }
    public float Angle { get; set; }

    public void SetPlanetRadius(float radius)
    {
        var scaledRadius = radius * this.SizeScaleFactor * 50f;
        this.transform.localScale = new Vector3(scaledRadius, scaledRadius, scaledRadius);
    }

    private void Rotate()
    {
        var scaledRotationSpeed = this.RotationSpeedKms * this.SpeedScaleFactor;
        this.transform.Rotate(Vector3.up, scaledRotationSpeed * Time.deltaTime);
    }

    private void Orbit()
    {
        if (this.OrbitCenter == null)
            return;

        var scaledOrbitSpeed = this.OrbitSpeedKms * this.SpeedScaleFactor;
        this.Angle += scaledOrbitSpeed * Time.deltaTime;
        var scaledOrbitRadius = this.OrbitRadiusKm * this.SizeScaleFactor;
        var x = Mathf.Cos(this.Angle) * scaledOrbitRadius;
        var z = Mathf.Sin(this.Angle) * scaledOrbitRadius;

        this.transform.position = new Vector3(x, this.transform.position.y, z) + this.OrbitCenter.position;

    }

    protected virtual void Start()
    {
        Debug.Log($"{this.name}: OrbitRadiusKm: {this.OrbitRadiusKm} km, OrbitSpeedKms: {this.OrbitSpeedKms} km/s, RotationSpeedKms: {this.RotationSpeedKms} km/s, Angle: {this.Angle} rad");
    }

    protected virtual void Update()
    {
        this.Rotate();
        this.Orbit();
    }
}