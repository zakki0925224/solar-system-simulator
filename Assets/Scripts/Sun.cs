public class Sun : Planet
{
    void Start()
    {
        this.OrbitRadiusKm = 0.0f;
        this.OrbitSpeedKms = 0.0f;
        this.RotationSpeedKms = 2.0f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(696340.0f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
