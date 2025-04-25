public class Neptune : Planet
{
    void Start()
    {
        this.OrbitRadiusKm = 4498396441.0f;
        this.OrbitSpeedKms = 5.43f;
        this.RotationSpeedKms = 2.68f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(24622.0f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
