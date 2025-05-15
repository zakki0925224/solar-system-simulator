public class Mars : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 227943824.0f;
        this.OrbitSpeedKms = 24.077f;
        this.RotationSpeedKms = -0.241f;
        this.AngleDeg = 25.2f;
        this.SetPlanetRadius(3389.5f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
