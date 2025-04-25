public class Uranus : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 2870658186.0f;
        this.OrbitSpeedKms = 6.81f;
        this.RotationSpeedKms = 0.09f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(25362.0f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
