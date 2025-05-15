public class Neptune : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 4498396441.0f;
        this.OrbitSpeedKms = 5.43f;
        this.RotationSpeedKms = -0.1f;
        this.AngleDeg = 28.3f;
        this.SetPlanetRadius(24622.0f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
