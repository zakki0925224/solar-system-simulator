using System;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public GameObject PlanetPrefab;

    public DateTime DateTime = DateTime.MinValue;

    public readonly float SizeScaleFactor = 1f / 100000000f;
    public readonly float SpeedScaleFactor = 8640f;

    void Start()
    {
        // generate planets
        var sunObject = CreatePlanet("Sun", null, Color.black);
        var earthObject = CreatePlanet("Earth", sunObject.transform, Color.blue);
        CreatePlanet("Moon", earthObject.transform, Color.gray);
        CreatePlanet("Mercury", sunObject.transform, Color.red);
        CreatePlanet("Venus", sunObject.transform, Color.yellow);
        CreatePlanet("Mars", sunObject.transform, new Color(1f, 0.5f, 0f));
        CreatePlanet("Jupiter", sunObject.transform, Color.magenta);
        CreatePlanet("Saturn", sunObject.transform, Color.cyan);
        CreatePlanet("Uranus", sunObject.transform, Color.green);
        CreatePlanet("Neptune", sunObject.transform, Color.blue);
    }

    private GameObject CreatePlanet(string name, Transform orbitCenter, Color trailColor)
    {
        var planetObject = Instantiate(this.PlanetPrefab);
        planetObject.name = name;

        // material
        var material = Resources.Load<Material>($"Materials/{name}");
        if (material != null)
        {
            planetObject.GetComponent<Renderer>().material = material;
        }
        else
        {
            Debug.LogWarning($"Material for {name} not found. Using default material.");
        }

        // trail
        var trailRenderer = planetObject.AddComponent<TrailRenderer>();
        trailRenderer.startWidth = 1.0f;
        trailRenderer.endWidth = 0f;
        trailRenderer.time = 50f;
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.startColor = trailColor;
        trailRenderer.endColor = new Color(trailColor.r, trailColor.g, trailColor.b, 0f);

        var planetType = System.Type.GetType(name);
        if (planetType != null)
        {
            var planet = planetObject.AddComponent(planetType) as Planet;
            planet.OrbitCenter = orbitCenter;
            planet.SizeScaleFactor = this.SizeScaleFactor;
            planet.SpeedScaleFactor = this.SpeedScaleFactor;
        }
        else
        {
            throw new System.Exception($"Planet type {name} not found.");
        }

        return planetObject;
    }

    void Update()
    {
        this.DateTime = this.DateTime.AddSeconds(Time.deltaTime * this.SpeedScaleFactor);
    }
}
