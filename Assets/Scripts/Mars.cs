public class Mars : Planet
{
    void Start()
    {
        this.OrbitRadius = 20.0f;
        this.OrbitSpeed = 0.07f;
        this.RotationSpeed = 12.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
