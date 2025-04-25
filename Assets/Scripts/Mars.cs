public class Mars : Planet
{
    void Start()
    {
        this.OrbitRadiusKm = 227943824.0f;
        this.OrbitSpeedKms = 24.077f;
        this.RotationSpeedKms = 0.241f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(3389.5f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
