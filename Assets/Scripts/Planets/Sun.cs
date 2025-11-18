using UnityEngine;

public class Sun : Planet
{
    private LensFlareEffect lensFlare;
    public float EmissiveIntensity = 2f;

    protected override void Start()
    {
        this.OrbitRadiusKm = 0.0f;
        this.OrbitSpeedKms = 0.0f;
        this.RotationSpeedKms = -1.997f;
        this.AngleDeg = 0.0f;
        this.SetPlanetRadius(696340.0f);

        base.Start();

        SetupEmissiveMaterial();

        SetupSunLight();

        SetupLensFlare();
    }

    private void SetupSunLight()
    {
        var lightObject = new GameObject("SunLight");
        lightObject.transform.parent = this.transform;
        lightObject.transform.localPosition = Vector3.zero;
        lightObject.transform.localRotation = Quaternion.identity;

        var light = lightObject.AddComponent<Light>();
        light.type = LightType.Directional;
        light.color = new Color(1f, 0.95f, 0.8f);
        light.intensity = 1.5f;
        light.shadows = LightShadows.Soft;
        light.shadowStrength = 0.8f;
        light.shadowNormalBias = 0.4f;
        light.shadowBias = 0.05f;

        light.cullingMask = ~(1 << this.gameObject.layer);

        lightObject.AddComponent<SunLightControl>();
    }

    private void SetupEmissiveMaterial()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null && renderer.material != null)
        {
            var material = renderer.material;

            material.EnableKeyword("_EMISSION");

            if (material.HasProperty("_EmissionColor"))
            {
                material.SetColor("_EmissionColor", Color.white * this.EmissiveIntensity);
            }

            if (material.HasProperty("_EmissionMap") && material.mainTexture != null)
            {
                material.SetTexture("_EmissionMap", material.mainTexture);
            }

            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

            Debug.Log($"{this.name}: Emissive material setup complete");
        }
    }

    private void SetupLensFlare()
    {
        this.lensFlare = this.gameObject.AddComponent<LensFlareEffect>();
        this.lensFlare.MainCamera = Camera.main;
        this.lensFlare.TargetObject = this.gameObject;
        this.lensFlare.FlareSize = 300f;
        this.lensFlare.MaxBrightness = 1.2f;
        this.lensFlare.FlareColor = new Color(1f, 0.95f, 0.8f, 1f);

        CreateFlareTexture();
    }

    private void CreateFlareTexture()
    {
        int size = 128;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);

        Vector2 center = new Vector2(size / 2f, size / 2f);
        float maxRadius = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2 pos = new Vector2(x, y);
                float distance = Vector2.Distance(pos, center);
                float alpha = Mathf.Clamp01(1f - (distance / maxRadius));
                alpha = Mathf.Pow(alpha, 2);

                texture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
            }
        }

        texture.Apply();
        this.lensFlare.FlareTexture = texture;
    }
}

