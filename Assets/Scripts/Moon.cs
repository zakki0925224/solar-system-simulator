public class Moon : Planet
{
    void Start()
    {
        this.OrbitRadius = 2.0f;
        this.OrbitSpeed = 0.2f;
        this.RotationSpeed = 5.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
