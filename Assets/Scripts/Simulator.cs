using System;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public GameObject PlanetPrefab;

    public DateTime DateTime = DateTime.MinValue;

    public float SizeScaleFactor = 1f / 100000000f;

    private float _speedScaleFactor = 86400f; // 1day
    public float SpeedScaleFactor
    {
        get => this._speedScaleFactor;
        set
        {
            this._speedScaleFactor = value;
            var planets = FindObjectsByType<Planet>(FindObjectsSortMode.None);
            foreach (var planet in planets)
            {
                planet.SpeedScaleFactor = value;
            }
        }
    }

    void Start()
    {
        // generate planets
        var sunObject = CreatePlanet("Sun", null, Color.black);
        sunObject.layer = LayerMask.NameToLayer("TransparentFX");

        var earthObject = CreatePlanet("Earth", sunObject.transform, new Color(0.2f, 0.7f, 1f, 1f));
        CreatePlanet("Moon", earthObject.transform, new Color(0.8f, 0.8f, 0.8f, 0.8f));
        CreatePlanet("Mercury", sunObject.transform, new Color(0.7f, 0.7f, 0.7f, 1f));
        CreatePlanet("Venus", sunObject.transform, new Color(1f, 0.9f, 0.4f, 1f));
        CreatePlanet("Mars", sunObject.transform, new Color(1f, 0.3f, 0.1f, 1f));
        CreatePlanet("Jupiter", sunObject.transform, new Color(1f, 0.6f, 0.3f, 1f));
        CreatePlanet("Saturn", sunObject.transform, new Color(1f, 0.85f, 0.5f, 1f));
        CreatePlanet("Uranus", sunObject.transform, new Color(0.4f, 0.9f, 1f, 1f));
        CreatePlanet("Neptune", sunObject.transform, new Color(0.2f, 0.4f, 1f, 1f));
    }

    private GameObject CreatePlanet(string name, Transform orbitCenter, Color orbitColor)
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

        var planetType = System.Type.GetType(name);
        if (planetType != null)
        {
            var planet = planetObject.AddComponent(planetType) as Planet;
            planet.OrbitCenter = orbitCenter;
            planet.SizeScaleFactor = this.SizeScaleFactor;
            planet.SpeedScaleFactor = this.SpeedScaleFactor;
            planet.OrbitColor = orbitColor;
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
