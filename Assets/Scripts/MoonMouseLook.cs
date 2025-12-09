using UnityEngine;
using UnityEngine.InputSystem;

public class MoonMouseLook : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    public float mouseSensitivity = 100f;

    [Header("References")]
    public Transform playerBody;

    private float xRotation = 0f;
    private InputAction lookAction;
    private InputAction escapeAction;

    void Awake()
    {
        lookAction = new InputAction(type: InputActionType.Value, binding: "<Mouse>/delta");
        lookAction.Enable();

        escapeAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/escape");
        escapeAction.Enable();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 mouseDelta = lookAction.ReadValue<Vector2>();
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }

        if (escapeAction.triggered)
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    void OnDestroy()
    {
        lookAction?.Dispose();
        escapeAction?.Dispose();
    }
}
