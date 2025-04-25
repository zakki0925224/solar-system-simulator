public class Uranus : Planet
{
    void Start()
    {
        this.OrbitRadiusKm = 2870658186.0f;
        this.OrbitSpeedKms = 6.81f;
        this.RotationSpeedKms = 2.59f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(25362.0f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
