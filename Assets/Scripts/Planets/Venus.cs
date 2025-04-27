using UnityEngine;

public class Venus : Planet
{
    protected override void Start()
    {
        this.OrbitRadiusKm = 108208000.0f;
        this.OrbitSpeedKms = 35.02f;
        this.RotationSpeedKms = -0.0018f;
        this.AngleDeg = 177f;
        this.SetPlanetRadius(6051.8f);

        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
}
