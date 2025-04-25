using UnityEngine;

public class Venus : Planet
{
    void Start()
    {
        this.OrbitRadius = 15.0f;
        this.OrbitSpeed = 0.08f;
        this.RotationSpeed = 10.0f;
        this.Angle = 0.0f;
    }

    protected override void Update()
    {
        base.Update();
    }
}
