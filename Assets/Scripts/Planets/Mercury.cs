public class Mercury : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 57909227.0f;
        this.OrbitSpeedKms = 47.87f;
        this.RotationSpeedKms = 0.003f;
        this.AngleDeg = 0.01f;
        this.SetPlanetRadius(2439.7f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
