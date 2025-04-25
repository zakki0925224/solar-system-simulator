public class Jupiter : Planet
{
    void Start()
    {
        this.OrbitRadiusKm = 778340821.0f;
        this.OrbitSpeedKms = 13.07f;
        this.RotationSpeedKms = 12.6f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(69911.0f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
