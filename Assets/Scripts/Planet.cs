using UnityEngine;

public class Planet : MonoBehaviour
{
    public Transform OrbitCenter { get; set; }
    public float OrbitRadius { get; set; }
    public float OrbitSpeed { get; set; }

    public float RotationSpeed { get; set; }
    public float Angle { get; set; }

    private void Rotate()
    {
        this.transform.Rotate(Vector3.up, this.RotationSpeed * Time.deltaTime);
    }

    private void Orbit()
    {
        if (this.OrbitCenter == null)
            return;

        this.Angle += this.OrbitSpeed * Time.deltaTime;
        var x = Mathf.Cos(this.Angle) * this.OrbitRadius;
        var z = Mathf.Sin(this.Angle) * this.OrbitRadius;

        this.transform.position = new Vector3(x, this.transform.position.y, z) + this.OrbitCenter.position;

    }

    protected virtual void Update()
    {
        this.Rotate();
        this.Orbit();
    }
}