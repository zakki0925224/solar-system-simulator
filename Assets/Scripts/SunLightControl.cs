using UnityEngine;

public class SunLightControl : MonoBehaviour
{
    private Transform sunTransform;
    private Light directionalLight;
    private Camera mainCamera;

    void Start()
    {
        directionalLight = GetComponent<Light>();
        if (directionalLight == null || directionalLight.type != LightType.Directional)
        {
            Debug.LogWarning("SunLightController: Directional Light component not found!");
            enabled = false;
            return;
        }

        sunTransform = transform.parent;
        if (sunTransform == null)
        {
            Debug.LogWarning("SunLightController: Parent (Sun) transform not found!");
            enabled = false;
            return;
        }

        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null || sunTransform == null)
            return;

        Vector3 directionToCamera = (mainCamera.transform.position - sunTransform.position).normalized;
        transform.rotation = Quaternion.LookRotation(directionToCamera);
    }
}
