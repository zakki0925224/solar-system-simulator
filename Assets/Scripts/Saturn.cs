public class Saturn : Planet
{
    void Start()
    {
        this.OrbitRadiusKm = 1426666422.0f;
        this.OrbitSpeedKms = 9.69f;
        this.RotationSpeedKms = 9.87f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(58232.0f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
