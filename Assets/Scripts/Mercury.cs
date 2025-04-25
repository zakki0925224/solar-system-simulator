public class Mercury : Planet
{
    void Start()
    {
        this.OrbitRadius = 5.0f;
        this.OrbitSpeed = 0.15f;
        this.RotationSpeed = 20.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
