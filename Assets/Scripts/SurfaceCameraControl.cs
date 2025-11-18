using UnityEngine;

public class SurfaceCameraControl : MonoBehaviour
{
    public Camera MainCamera;
    public Transform EarthTransform;
    public Simulator Simulator;
    public float Latitude = 35.6762f;
    public float Longitude = 139.6503f;
    public float HeightAboveSurfaceKm = 2f;
    public bool IsActive = false;

    private CameraControl cameraControl;

    void Start()
    {
        if (MainCamera == null)
        {
            Debug.LogError("SurfaceCameraControl: MainCamera is not assigned!");
            enabled = false;
            return;
        }

        if (Simulator == null)
        {
            Simulator = FindFirstObjectByType<Simulator>();
            if (Simulator == null)
            {
                Debug.LogError("SurfaceCameraControl: Simulator not found!");
                enabled = false;
                return;
            }
        }

        cameraControl = MainCamera.GetComponent<CameraControl>();
        if (cameraControl == null)
        {
            Debug.LogWarning("SurfaceCameraControl: CameraControl component not found on MainCamera");
        }

        Debug.Log("SurfaceCameraControl: Initialized");
    }

    void Update()
    {
        if (EarthTransform == null)
        {
            var earthObj = GameObject.Find("Earth");
            if (earthObj != null)
            {
                EarthTransform = earthObj.transform;
                Debug.Log($"SurfaceCameraControl: Earth found at {EarthTransform.position}");
            }
        }
    }

    void LateUpdate()
    {
        if (!IsActive || EarthTransform == null || MainCamera == null || Simulator == null)
            return;

        var earthRadius = EarthTransform.localScale.x / 2f;
        var latRad = Latitude * Mathf.Deg2Rad;
        var lonRad = Longitude * Mathf.Deg2Rad;

        var normalizedPosition = new Vector3(
            Mathf.Cos(latRad) * Mathf.Cos(lonRad),
            Mathf.Sin(latRad),
            Mathf.Cos(latRad) * Mathf.Sin(lonRad)
        );

        var earthComponent = EarthTransform.GetComponent<Earth>();
        if (earthComponent == null)
        {
            Debug.LogError("SurfaceCameraControl: Earth component not found!");
            return;
        }

        var earthRadiusKm = earthComponent.RadiusKm;
        var heightScaled = (HeightAboveSurfaceKm / earthRadiusKm) * earthRadius;

        var localPosition = normalizedPosition * (earthRadius + heightScaled);

        var worldPosition = EarthTransform.position + EarthTransform.rotation * localPosition;

        MainCamera.transform.position = worldPosition;

        Vector3 upDirection = (MainCamera.transform.position - EarthTransform.position).normalized;

        Vector3 worldNorth = EarthTransform.TransformDirection(Vector3.up);

        Vector3 eastDirection = Vector3.Cross(upDirection, worldNorth).normalized;

        Vector3 southDirection = -Vector3.Cross(eastDirection, upDirection).normalized;

        Vector3 lookDirection = (southDirection + upDirection * 0.5f).normalized;

        MainCamera.transform.rotation = Quaternion.LookRotation(lookDirection, upDirection);
    }

    public void SetActive(bool value)
    {
        IsActive = value;

        if (cameraControl != null)
        {
            cameraControl.enabled = !IsActive;
        }

        if (IsActive)
        {
            Debug.Log("Surface camera mode: ENABLED");
        }
        else
        {
            Debug.Log("Surface camera mode: DISABLED");
        }
    }
}
