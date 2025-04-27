public class Sun : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 0.0f;
        this.OrbitSpeedKms = 0.0f;
        this.RotationSpeedKms = 1.997f;
        this.AngleDeg = 0.0f;
        this.SetPlanetRadius(696340.0f);

        base.Start();
    }
}

