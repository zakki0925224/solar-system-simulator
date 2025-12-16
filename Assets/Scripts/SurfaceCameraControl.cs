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
            enabled = false;
            return;
        }

        if (Simulator == null)
        {
            Simulator = FindFirstObjectByType<Simulator>();
            if (Simulator == null)
            {
                enabled = false;
                return;
            }
        }

        cameraControl = MainCamera.GetComponent<CameraControl>();
    }

    void Update()
    {
        if (EarthTransform == null)
        {
            var earthObj = GameObject.Find("Earth");
            if (earthObj != null)
            {
                EarthTransform = earthObj.transform;
            }
        }

        if (IsActive && MainCamera != null && EarthTransform != null)
        {
            CalculateCameraPosition();
        }
    }

    void LateUpdate()
    {
        if (!IsActive || EarthTransform == null || MainCamera == null || Simulator == null)
            return;

        CalculateCameraPosition();
    }

    public void SetActive(bool value)
    {
        IsActive = value;

        if (cameraControl != null)
        {
            cameraControl.enabled = !IsActive;

            if (IsActive)
            {
                cameraControl.DisableInput();
            }
            else
            {
                cameraControl.EnableInput();
            }
        }

        if (IsActive)
        {
            CalculateCameraPosition();
        }
    }

    private void CalculateCameraPosition()
    {
        if (EarthTransform == null || MainCamera == null || Simulator == null)
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

        var earthRadiusKm = earthComponent.RadiusKm;
        var heightScaled = (HeightAboveSurfaceKm / earthRadiusKm) * earthRadius;

        var localPosition = normalizedPosition * (earthRadius + heightScaled);

        var worldPosition = EarthTransform.position + EarthTransform.rotation * localPosition;

        Vector3 upDirection = (worldPosition - EarthTransform.position).normalized;

        Vector3 worldNorth = EarthTransform.TransformDirection(Vector3.up);

        Vector3 eastDirection = Vector3.Cross(upDirection, worldNorth).normalized;

        Vector3 southDirection = -Vector3.Cross(eastDirection, upDirection).normalized;

        Vector3 lookDirection = (southDirection + upDirection * 0.5f).normalized;

        MainCamera.transform.position = worldPosition;
        MainCamera.transform.rotation = Quaternion.LookRotation(lookDirection, upDirection);
    }
}
