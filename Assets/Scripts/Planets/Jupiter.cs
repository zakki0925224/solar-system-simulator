public class Jupiter : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 778340821.0f;
        this.OrbitSpeedKms = 13.07f;
        this.RotationSpeedKms = 0.21f;
        this.AngleDeg = 26.7f;
        this.SetPlanetRadius(69911.0f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
