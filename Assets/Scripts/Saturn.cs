public class Saturn : Planet
{
    void Start()
    {
        this.OrbitRadius = 40.0f;
        this.OrbitSpeed = 0.03f;
        this.RotationSpeed = 6.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
