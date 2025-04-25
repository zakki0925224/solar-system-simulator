public class Neptune : Planet
{
    void Start()
    {
        this.OrbitRadius = 60.0f;
        this.OrbitSpeed = 0.01f;
        this.RotationSpeed = 4.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
