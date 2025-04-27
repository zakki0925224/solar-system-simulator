public class Earth : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 149597870.7f;
        this.OrbitSpeedKms = 29.78f;
        this.RotationSpeedKms = 0.465f;
        this.AngleDeg = 23.4f;
        this.SetPlanetRadius(6371.0f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
