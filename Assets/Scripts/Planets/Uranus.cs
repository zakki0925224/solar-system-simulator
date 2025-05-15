public class Uranus : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 2870658186.0f;
        this.OrbitSpeedKms = 6.81f;
        this.RotationSpeedKms = -0.09f;
        this.AngleDeg = 97.8f;
        this.SetPlanetRadius(25362.0f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
