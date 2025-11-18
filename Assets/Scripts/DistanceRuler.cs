using UnityEngine;

public class DistanceRuler : MonoBehaviour
{
    public Camera MainCamera;
    public Color RulerColor = new Color(1f, 1f, 1f, 0.5f);

    private LineRenderer mainLine;
    private CameraControl cameraControl;
    private Transform currentFollowerObject;
    private Transform sunTransform;
    private Simulator simulator;

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
        mainLine.startWidth = 0.1f;
        mainLine.endWidth = 0.1f;
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
    }
}