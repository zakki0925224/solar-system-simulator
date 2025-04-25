using UnityEngine;

public class Venus : Planet
{
    void Start()
    {
        this.OrbitRadiusKm = 108208000.0f;
        this.OrbitSpeedKms = 35.02f;
        this.RotationSpeedKms = -0.0018f;
        this.Angle = 0.0f;
        this.SetPlanetRadius(6051.8f);
    }

    protected override void Update()
    {
        base.Update();
    }
}
