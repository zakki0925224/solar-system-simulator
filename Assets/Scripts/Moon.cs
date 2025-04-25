public class Moon : Planet
{
    void Start()
    {
        this.OrbitRadiusKm = 384400.0f;
        this.OrbitSpeedKms = 1.022f;
        this.RotationSpeedKms = 0.0046f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(1737.4f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
