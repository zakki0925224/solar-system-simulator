using UnityEngine;

public class OrbitPath : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Planet planet;
    private readonly int segments = 256;
    private float lastWidth = -1f;

    public void Initialize(Planet planet, Color color, float width = 0.2f)
    {
        this.planet = planet;

        this.lineRenderer = this.gameObject.AddComponent<LineRenderer>();

        var material = new Material(Shader.Find("Sprites/Default"));
        material.color = color;
        this.lineRenderer.material = material;
        this.lineRenderer.startColor = color;
        this.lineRenderer.endColor = color;
        this.lineRenderer.startWidth = width;
        this.lineRenderer.endWidth = width;
        this.lineRenderer.positionCount = this.segments + 1;
        this.lineRenderer.useWorldSpace = false;
        this.lineRenderer.loop = true;
        this.lineRenderer.numCornerVertices = 10;
        this.lineRenderer.numCapVertices = 10;
        this.lineRenderer.textureMode = LineTextureMode.Tile;
        this.lineRenderer.alignment = LineAlignment.View;
        this.lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        this.lineRenderer.receiveShadows = false;

        this.DrawOrbit();
    }

    private void DrawOrbit()
    {
        if (this.planet.OrbitCenter == null)
            return;

        var orbitRadiusM = this.planet.OrbitRadiusKm * 1000f;
        var scaledRadius = orbitRadiusM * this.planet.SizeScaleFactor;

        for (int i = 0; i <= this.segments; i++)
        {
            var angle = i * 360f / this.segments * Mathf.Deg2Rad;
            var x = Mathf.Cos(angle) * scaledRadius;
            var z = Mathf.Sin(angle) * scaledRadius;
            var position = new Vector3(x, 0f, z);
            this.lineRenderer.SetPosition(i, position);
        }
    }

    void Update()
    {
        if (Camera.main != null && this.planet != null)
        {
            var distance = Vector3.Distance(Camera.main.transform.position, this.planet.transform.position);
            var targetWidth = Mathf.Clamp(distance / 500f, 0.1f, 3f);

            if (this.lastWidth < 0f)
            {
                this.lastWidth = targetWidth;
            }
            else
            {
                this.lastWidth = Mathf.Lerp(this.lastWidth, targetWidth, Time.deltaTime * 2f);
            }

            if (Mathf.Abs(this.lastWidth - this.lineRenderer.startWidth) > 0.01f)
            {
                this.lineRenderer.startWidth = this.lastWidth;
                this.lineRenderer.endWidth = this.lastWidth;
            }
        }
    }
}
