using UnityEngine;

public class Main : MonoBehaviour
{
    public GameObject PlanetPrefab;

    void Start()
    {
        // generate planets
        var sunObject = Instantiate(this.PlanetPrefab);
        sunObject.name = "Sun";
        sunObject.AddComponent<Sun>();

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

        var trailRenderer = planetObject.AddComponent<TrailRenderer>();
        trailRenderer.startWidth = 0.5f;
        trailRenderer.endWidth = 0f;
        trailRenderer.time = 100f;
        trailRenderer.material = new Material(Shader.Find("Sprites/Default"));
        trailRenderer.startColor = trailColor;
        trailRenderer.endColor = new Color(trailColor.r, trailColor.g, trailColor.b, 0f);

        var planetType = System.Type.GetType(name);
        if (planetType != null)
        {
            var planet = planetObject.AddComponent(planetType) as Planet;
            planet.OrbitCenter = orbitCenter;
        }
        else
        {
            throw new System.Exception($"Planet type {name} not found.");
        }

        return planetObject;
    }
}
