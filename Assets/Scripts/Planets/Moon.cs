public class Moon : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 384400.0f;
        this.OrbitSpeedKms = 1.022f;
        this.RotationSpeedKms = -0.0046f;
        this.AngleDeg = 6.7f;
        this.SetPlanetRadius(1737.4f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
