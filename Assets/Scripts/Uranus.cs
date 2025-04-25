public class Uranus : Planet
{
    void Start()
    {
        this.OrbitRadius = 50.0f;
        this.OrbitSpeed = 0.02f;
        this.RotationSpeed = 5.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
