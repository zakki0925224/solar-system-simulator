public class Saturn : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 1426666422.0f;
        this.OrbitSpeedKms = 9.69f;
        this.RotationSpeedKms = 0.09f;
        this.AngleDeg = 3.12f;
        this.SetPlanetRadius(58232.0f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
