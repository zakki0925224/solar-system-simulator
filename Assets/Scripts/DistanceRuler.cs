using UnityEngine;

public class DistanceRuler : MonoBehaviour
{
    [Header("References")]
    public Camera MainCamera;

    [Header("Ruler Settings")]
    public Color RulerColor = new Color(1f, 1f, 1f, 0.5f);
    public Color TickColor = new Color(1f, 1f, 0f, 1f);

    private LineRenderer mainLine;
    private LineRenderer[] tickLines;
    private int currentTickCount = 0;
    private CameraControl cameraControl;
    private Transform currentFollowerObject;
    private Transform sunTransform;
    private Simulator simulator;

    private const float AU_IN_KM = 149597870.7f;

    void Start()
    {
        if (MainCamera == null)
        {
            MainCamera = Camera.main;
        }

        cameraControl = FindFirstObjectByType<CameraControl>();
        if (cameraControl == null)
        {
            Debug.LogWarning("DistanceRuler: CameraControl not found!");
        }

        simulator = FindFirstObjectByType<Simulator>();
        if (simulator == null)
        {
            Debug.LogWarning("DistanceRuler: Simulator not found!");
        }

        GameObject mainLineObj = new GameObject("RulerMainLine");
        mainLineObj.transform.SetParent(transform);
        mainLine = mainLineObj.AddComponent<LineRenderer>();

        var material = new Material(Shader.Find("Sprites/Default"));
        material.color = RulerColor;

        mainLine.material = material;
        mainLine.startColor = RulerColor;
        mainLine.endColor = RulerColor;
        mainLine.startWidth = 0.3f;
        mainLine.endWidth = 0.3f;
        mainLine.positionCount = 2;
        mainLine.useWorldSpace = true;
        mainLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mainLine.receiveShadows = false;

        mainLine.enabled = false;
    }

    void Update()
    {
        if (sunTransform == null)
        {
            Sun sun = FindFirstObjectByType<Sun>();
            if (sun != null)
            {
                sunTransform = sun.transform;
                Debug.Log("DistanceRuler: Sun found and set");
            }
        }

        if (cameraControl != null && cameraControl.FollowerObject != null)
        {
            currentFollowerObject = cameraControl.FollowerObject.transform;
        }
        else
        {
            currentFollowerObject = null;
        }

        if (currentFollowerObject == null || sunTransform == null)
        {
            if (mainLine != null)
            {
                mainLine.enabled = false;
            }
            HideAllTicks();
            return;
        }

        mainLine.enabled = true;
        mainLine.SetPosition(0, sunTransform.position);
        mainLine.SetPosition(1, currentFollowerObject.position);

        if (MainCamera != null)
        {
            float distance = Vector3.Distance(MainCamera.transform.position, sunTransform.position);
            float targetWidth = Mathf.Clamp(distance / 500f, 0.1f, 3f);
            mainLine.startWidth = targetWidth;
            mainLine.endWidth = targetWidth;
        }

        UpdateTicks();
    }

    void UpdateTicks()
    {
        if (sunTransform == null || currentFollowerObject == null || simulator == null) return;

        Vector3 startPos = sunTransform.position;
        Vector3 endPos = currentFollowerObject.position;
        float totalDistance = Vector3.Distance(startPos, endPos);

        float realDistanceKm = totalDistance / simulator.SizeScaleFactor;

        float distanceAU = realDistanceKm / AU_IN_KM;
        float tickIntervalAU;

        if (distanceAU < 1f)
        {
            tickIntervalAU = 0.1f;
        }
        else if (distanceAU < 5f)
        {
            tickIntervalAU = 0.5f;
        }
        else if (distanceAU < 20f)
        {
            tickIntervalAU = 2f;
        }
        else if (distanceAU < 50f)
        {
            tickIntervalAU = 5f;
        }
        else
        {
            tickIntervalAU = 10f;
        }

        float tickIntervalUnity = tickIntervalAU * AU_IN_KM * simulator.SizeScaleFactor;

        int tickCount = Mathf.FloorToInt(totalDistance / tickIntervalUnity);
        tickCount = Mathf.Min(tickCount, 100);

        if (tickLines == null || tickLines.Length < tickCount)
        {
            if (tickLines != null)
            {
                foreach (var tick in tickLines)
                {
                    if (tick != null)
                    {
                        Destroy(tick.gameObject);
                    }
                }
            }

            tickLines = new LineRenderer[tickCount];
            var tickMaterial = new Material(Shader.Find("Sprites/Default"));
            tickMaterial.color = TickColor;

            for (int i = 0; i < tickCount; i++)
            {
                GameObject tickObj = new GameObject($"RulerTick_{i}");
                tickObj.transform.SetParent(transform);

                LineRenderer lr = tickObj.AddComponent<LineRenderer>();
                lr.material = tickMaterial;
                lr.startColor = TickColor;
                lr.endColor = TickColor;
                lr.startWidth = 0.3f;
                lr.endWidth = 0.3f;
                lr.positionCount = 2;
                lr.useWorldSpace = true;
                lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                lr.receiveShadows = false;

                tickLines[i] = lr;
            }
        }

        currentTickCount = tickCount;

        Vector3 direction = (endPos - startPos).normalized;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized;

        if (perpendicular.magnitude < 0.01f)
        {
            perpendicular = Vector3.Cross(direction, Vector3.forward).normalized;
        }

        for (int i = 0; i < tickCount; i++)
        {
            if (tickLines[i] != null)
            {
                tickLines[i].enabled = true;

                float distanceAlongLine = (i + 1) * tickIntervalUnity;
                Vector3 tickCenter = startPos + direction * distanceAlongLine;

                float cameraDistance = Vector3.Distance(MainCamera.transform.position, tickCenter);
                float scale = Mathf.Clamp(cameraDistance / 500f, 0.1f, 3f);

                float adjustedTickLength = 5f * scale;
                float adjustedTickWidth = scale * 1.5f;

                Vector3 tickStart = tickCenter - perpendicular * adjustedTickLength * 0.5f;
                Vector3 tickEnd = tickCenter + perpendicular * adjustedTickLength * 0.5f;

                tickLines[i].SetPosition(0, tickStart);
                tickLines[i].SetPosition(1, tickEnd);
                tickLines[i].startWidth = adjustedTickWidth;
                tickLines[i].endWidth = adjustedTickWidth;
            }
        }

        if (tickLines != null)
        {
            for (int i = tickCount; i < tickLines.Length; i++)
            {
                if (tickLines[i] != null)
                {
                    tickLines[i].enabled = false;
                }
            }
        }
    }

    void HideAllTicks()
    {
        if (tickLines != null)
        {
            foreach (var tick in tickLines)
            {
                if (tick != null)
                {
                    tick.enabled = false;
                }
            }
        }
    }

    void OnDestroy()
    {
        if (tickLines != null)
        {
            foreach (var tick in tickLines)
            {
                if (tick != null)
                {
                    Destroy(tick.gameObject);
                }
            }
        }
    }
}
