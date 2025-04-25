public class Jupiter : Planet
{
    void Start()
    {
        this.OrbitRadius = 30.0f;
        this.OrbitSpeed = 0.05f;
        this.RotationSpeed = 8.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
