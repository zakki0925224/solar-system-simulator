using UnityEngine;

public class Planet : MonoBehaviour
{
    public float SizeScaleFactor { get; set; }
    public float SpeedScaleFactor { get; set; }

    public Transform OrbitCenter { get; set; }
    public float OrbitRadiusKm { get; set; } // km
    public float OrbitSpeedKms { get; set; } // km/s

    public float RadiusKm { get; set; } // km

    public float RotationSpeedKms { get; set; } // km/s
    public float AngleDeg { get; set; }

    public void SetPlanetRadius(float radiusKm)
    {
        this.RadiusKm = radiusKm;
        var scaledRadius = radiusKm * 1000f * this.SizeScaleFactor * 50f;
        this.transform.localScale = new Vector3(scaledRadius, scaledRadius, scaledRadius);
    }

    private void Rotate()
    {
        var selfRadiusM = this.RadiusKm * 1000f; // [m]
        var rotationSpeedMps = this.RotationSpeedKms * 1000f; // [m/s]
        var angularVelocityDeg = rotationSpeedMps / selfRadiusM * Mathf.Rad2Deg; // [deg/s]
        var rotationDeg = angularVelocityDeg * Time.deltaTime * this.SpeedScaleFactor;
        this.transform.Rotate(Vector3.up, rotationDeg);
    }

    private void Orbit()
    {
        if (this.OrbitCenter == null)
            return;

        var orbitRadiusM = this.OrbitRadiusKm * 1000f; // [m]
        var orbitSpeedMps = this.OrbitSpeedKms * 1000f; // [m/s]
        var angularVelocityDeg = orbitSpeedMps / orbitRadiusM * Mathf.Rad2Deg; // [deg/s]
        var deltaAngleDeg = angularVelocityDeg * Time.deltaTime * this.SpeedScaleFactor;
        this.AngleDeg += deltaAngleDeg;

        var x = Mathf.Cos(this.AngleDeg * Mathf.Deg2Rad) * orbitRadiusM * this.SizeScaleFactor;
        var z = Mathf.Sin(this.AngleDeg * Mathf.Deg2Rad) * orbitRadiusM * this.SizeScaleFactor;

        this.transform.position = this.OrbitCenter.position + new Vector3(x, 0f, z);
    }

    protected virtual void Start()
    {
        Debug.Log($"{this.name}: OrbitRadiusKm: {this.OrbitRadiusKm} km, OrbitSpeedKms: {this.OrbitSpeedKms} km/s, RotationSpeedKms: {this.RotationSpeedKms} km/s, AngleDeg: {this.AngleDeg}, RadiusKm: {this.RadiusKm} km");
    }

    protected virtual void Update()
    {
        this.Rotate();
        this.Orbit();
    }
}