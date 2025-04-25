public class Earth : Planet
{
    void Start()
    {
        this.OrbitRadiusKm = 149597870.7f;
        this.OrbitSpeedKms = 29.78f;
        this.RotationSpeedKms = 0.465f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(6371.0f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
