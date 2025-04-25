public class Earth : Planet
{
    void Start()
    {
        this.OrbitRadius = 10.0f;
        this.OrbitSpeed = 0.1f;
        this.RotationSpeed = 15.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
