public class Sun : Planet
{
    void Start()
    {
        this.OrbitRadius = 0.0f;
        this.OrbitSpeed = 0.0f;
        this.RotationSpeed = 20.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
