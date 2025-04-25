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

        var earthObject = Instantiate(this.PlanetPrefab);
        earthObject.name = "Earth";
        var earth = earthObject.AddComponent<Earth>();
        earth.OrbitCenter = sunObject.transform;

        var moonObject = Instantiate(this.PlanetPrefab);
        moonObject.name = "Moon";
        var moon = moonObject.AddComponent<Moon>();
        moon.OrbitCenter = earthObject.transform;

        var mercuryObject = Instantiate(this.PlanetPrefab);
        mercuryObject.name = "Mercury";
        var mercury = mercuryObject.AddComponent<Mercury>();
        mercury.OrbitCenter = sunObject.transform;

        var venusObject = Instantiate(this.PlanetPrefab);
        venusObject.name = "Venus";
        var venus = venusObject.AddComponent<Venus>();
        venus.OrbitCenter = sunObject.transform;

        var marsObject = Instantiate(this.PlanetPrefab);
        marsObject.name = "Mars";
        var mars = marsObject.AddComponent<Mars>();
        mars.OrbitCenter = sunObject.transform;

        var jupiterObject = Instantiate(this.PlanetPrefab);
        jupiterObject.name = "Jupiter";
        var jupiter = jupiterObject.AddComponent<Jupiter>();
        jupiter.OrbitCenter = sunObject.transform;

        var saturnObject = Instantiate(this.PlanetPrefab);
        saturnObject.name = "Saturn";
        var saturn = saturnObject.AddComponent<Saturn>();
        saturn.OrbitCenter = sunObject.transform;

        var uranusObject = Instantiate(this.PlanetPrefab);
        uranusObject.name = "Uranus";
        var uranus = uranusObject.AddComponent<Uranus>();
        uranus.OrbitCenter = sunObject.transform;

        var neptuneObject = Instantiate(this.PlanetPrefab);
        neptuneObject.name = "Neptune";
        var neptune = neptuneObject.AddComponent<Neptune>();
        neptune.OrbitCenter = sunObject.transform;
    }

    void Update()
    {

    }
}
